using CheapChic.Data;
using CheapChic.Data.Entities;
using CheapChic.Infrastructure.Bot.Requests;
using CheapChic.Infrastructure.Configuration.Models;
using CheapChic.Infrastructure.Constants;
using CheapChic.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace CheapChic.Infrastructure.Bot;

public class TelegramBot : ITelegramBot
{
    private readonly IOptions<ApplicationOptions> _applicationOptions;
    private readonly IOptions<ManagementBotOptions> _managementBotOptions;
    private readonly HttpClient _httpClient;
    private readonly CheapChicContext _context;
    private readonly IMemoryCache _memoryCache;

    public TelegramBot(HttpClient httpClient, IOptions<ApplicationOptions> applicationOptions, CheapChicContext context,
        IOptions<ManagementBotOptions> managementBotOptions, IMemoryCache memoryCache)
    {
        _httpClient = httpClient;
        _applicationOptions = applicationOptions;
        _context = context;
        _managementBotOptions = managementBotOptions;
        _memoryCache = memoryCache;
    }

    public async Task SetWebhook(string token, CancellationToken cancellationToken = default)
    {
        var host = _applicationOptions.Value.Host;
        var client = GetClient(token);

        var webhookUrl =
            $"{host}" +
            $"{(host.EndsWith("/") ? string.Empty : "/")}" +
            $"{ControllerName.Telegram}/" +
            $"{token}";

        var webhookInfo = await client.GetWebhookInfoAsync(cancellationToken: cancellationToken);
        if (webhookInfo.Url != webhookUrl)
        {
            await client.SetWebhookAsync(webhookUrl, cancellationToken: cancellationToken);
        }
    }

    public async Task DeleteWebhook(string token, CancellationToken cancellationToken = default)
    {
        var client = GetClient(token);

        var webhookInfo = await client.GetWebhookInfoAsync(cancellationToken: cancellationToken);
        if (!string.IsNullOrEmpty(webhookInfo.Url))
        {
            await client.DeleteWebhookAsync(cancellationToken: cancellationToken);
        }
    }

    public async Task<TelegramMessageEntity> SendText(string token, SendTextMessageRequest request,
        CancellationToken cancellationToken = default)
    {
        var chatId = request.ChatId;
        var text = request.Text;

        var (userId, channelId) = await GetUserChannelIds(chatId, cancellationToken);

        var botId = await GetTelegramBotId(token, cancellationToken);

        var client = GetClient(token);
        var message =
            await client.SendTextMessageAsync(chatId, text, ParseMode.Html, cancellationToken: cancellationToken);

        var telegramMessage = new TelegramMessageEntity
        {
            MessageId = message.MessageId,
            Type = TelegramMessageEntity.TelegramMessageType.Text,
            Message = request.ToJson(),
            UserId = userId,
            BotId = botId,
            ChannelId = channelId
        };

        await _context.AddAsync(telegramMessage, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return telegramMessage;
    }

    public async Task<TelegramMessageEntity> SendReplyKeyboard(string token, SendReplyKeyboardRequest request,
        CancellationToken cancellationToken = default)
    {
        var chatId = request.ChatId;
        var text = request.Text;
        var keyboard = request.Keyboard;

        var replyKeyboard = new ReplyKeyboardMarkup(keyboard.GetButtons())
        {
            Selective = false,
            ResizeKeyboard = true,
            InputFieldPlaceholder = text,
            OneTimeKeyboard = false
        };

        var userId = await GetUserId(chatId, cancellationToken);

        if (!userId.HasValue)
        {
            var exceptionMessage = ExceptionMessage.UserNotFound(chatId);
            throw new Exception(exceptionMessage);
        }

        var botId = await GetTelegramBotId(token, cancellationToken);

        var client = GetClient(token);
        var message = await client.SendTextMessageAsync(chatId, text, ParseMode.Html, replyMarkup: replyKeyboard,
            cancellationToken: cancellationToken);

        var telegramMessage = new TelegramMessageEntity
        {
            MessageId = message.MessageId,
            Type = TelegramMessageEntity.TelegramMessageType.ReplyKeyboard,
            Message = request.ToJson(),
            UserId = userId,
            BotId = botId,
            ChannelId = null
        };

        await _context.AddAsync(telegramMessage, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return telegramMessage;
    }

    public async Task<byte[]> DownloadFile(string token, string fileId, CancellationToken cancellationToken = default)
    {
        var client = GetClient(token);
        using var ms = new MemoryStream();
        await client.GetInfoAndDownloadFileAsync(fileId, ms, cancellationToken);
        return ms.ToArray();
    }

    public async Task<TelegramMessageEntity[]> SendPhoto(string token, SendMediaGroupRequest request,
        CancellationToken cancellationToken = default)
    {
        var chatId = request.ChatId;
        var photoIds = request.PhotoIds;

        var (userId, channelId) = await GetUserChannelIds(chatId, cancellationToken);

        var botId = await GetTelegramBotId(token, cancellationToken);

        var photoContents = await _context.Photos
            .AsNoTracking()
            .Where(x => photoIds.Contains(x.Id))
            .OrderBy(x => x.CreatedOn)
            .Select(x => x.Content)
            .ToListAsync(cancellationToken);

        var memoryStreams = new List<MemoryStream>();

        var mediaGroup = photoContents
            .Select(x =>
            {
                var ms = new MemoryStream(x);
                memoryStreams.Add(ms);
                return new InputMediaPhoto(new InputMedia(ms, Guid.NewGuid().ToString()));
            })
            .ToArray();

        if (mediaGroup.Length > 0)
        {
            mediaGroup[0].Caption = request.Text;
            mediaGroup[0].ParseMode = ParseMode.Html;
        }

        var client = GetClient(token);

        try
        {
            var messages = await client.SendMediaGroupAsync(chatId, mediaGroup, cancellationToken: cancellationToken);

            var telegramMessages = messages
                .Select(x => new TelegramMessageEntity
                {
                    MessageId = x.MessageId,
                    Type = TelegramMessageEntity.TelegramMessageType.Photo,
                    UserId = userId,
                    ChannelId = channelId,
                    BotId = botId,
                    Message = request.ToJson()
                })
                .ToArray();

            await _context.AddRangeAsync(telegramMessages, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return telegramMessages;
        }
        catch
        {
            return Array.Empty<TelegramMessageEntity>();
        }
        finally
        {
            foreach (var ms in memoryStreams)
            {
                await ms.DisposeAsync();
            }
        }
    }

    public async Task<bool> TestToken(string token, CancellationToken cancellationToken)
    {
        var client = GetClient(token);

        try
        {
            return await client.TestApiAsync(cancellationToken);
        }
        catch
        {
            return false;
        }
    }

    private async Task<(Guid? UserId, Guid? ChannelId)> GetUserChannelIds(long chatId,
        CancellationToken cancellationToken = default)
    {
        var userId = await GetUserId(chatId, cancellationToken);
        var channelId = await GetChannelId(chatId, cancellationToken);

        if (userId is null && channelId is null)
        {
            var exceptionMessage = ExceptionMessage.UserChannelNotFound(chatId);
            throw new Exception(exceptionMessage);
        }

        return (userId, channelId);
    }

    private async Task<Guid?> GetTelegramBotId(string token, CancellationToken cancellationToken = default)
    {
        if (token.Equals(_managementBotOptions.Value.Token, StringComparison.InvariantCultureIgnoreCase))
        {
            return null;
        }

        var cacheKey = GetBotCacheKey(token);

        if (!_memoryCache.TryGetValue(cacheKey, out Guid botId))
        {
            botId = await _context.TelegramBots
                .AsNoTracking()
                .Where(x => x.Token == token)
                .Select(x => x.Id)
                .FirstOrDefaultAsync(cancellationToken);

            if (botId == Guid.Empty)
            {
                var exceptionMessage = ExceptionMessage.BotNotFound;
                throw new Exception(exceptionMessage);
            }

            _memoryCache.Set(cacheKey, botId);
        }

        return botId;
    }

    private async Task<Guid?> GetUserId(long chatId, CancellationToken cancellationToken = default)
    {
        var cacheKey = GetUserCacheKey(chatId);

        if (!_memoryCache.TryGetValue(cacheKey, out Guid userId))
        {
            userId = await _context.TelegramUsers
                .AsNoTracking()
                .Where(x => x.ChatId == chatId)
                .Select(x => x.Id)
                .FirstOrDefaultAsync(cancellationToken);

            if (userId == Guid.Empty)
            {
                return null;
            }

            _memoryCache.Set(cacheKey, userId);
        }

        return userId;
    }

    private async Task<Guid?> GetChannelId(long chatId, CancellationToken cancellationToken = default)
    {
        var cacheKey = GetChannelCacheKey(chatId);

        if (!_memoryCache.TryGetValue(cacheKey, out Guid channelId))
        {
            channelId = await _context.TelegramChannels
                .AsNoTracking()
                .Where(x => x.ChatId == chatId)
                .Select(x => x.Id)
                .FirstOrDefaultAsync(cancellationToken);

            if (channelId == Guid.Empty)
            {
                return null;
            }

            _memoryCache.Set(cacheKey, channelId);
        }

        return channelId;
    }

    private static string GetChannelCacheKey(long chatId)
    {
        return $"channel_{chatId}";
    }

    private static string GetUserCacheKey(long chatId)
    {
        return $"user_{chatId}";
    }

    private static string GetBotCacheKey(string token)
    {
        return $"bot_{token}";
    }

    private TelegramBotClient GetClient(string token) => new(token, _httpClient);
}