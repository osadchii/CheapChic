using CheapChic.Data;
using CheapChic.Data.Entities;
using CheapChic.Infrastructure.Bot.Models;
using CheapChic.Infrastructure.Bot.Requests;
using CheapChic.Infrastructure.Configuration.Models;
using CheapChic.Infrastructure.Constants;
using CheapChic.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace CheapChic.Infrastructure.Bot;

public class TelegramBot : ITelegramBot
{
    private readonly IOptions<ApplicationOptions> _applicationOptions;
    private readonly IOptions<ManagementBotOptions> _managementBotOptions;
    private readonly HttpClient _httpClient;
    private readonly CheapChicContext _context;

    public TelegramBot(HttpClient httpClient, IOptions<ApplicationOptions> applicationOptions, CheapChicContext context,
        IOptions<ManagementBotOptions> managementBotOptions)
    {
        _httpClient = httpClient;
        _applicationOptions = applicationOptions;
        _context = context;
        _managementBotOptions = managementBotOptions;
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
            TelegramBotId = botId,
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
            OneTimeKeyboard = true
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
            TelegramBotId = botId,
            ChannelId = null
        };

        await _context.AddAsync(telegramMessage, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return telegramMessage;
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

        var botId = await _context.TelegramBots
            .Where(x => x.Token == token)
            .Select(x => x.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (botId == Guid.Empty)
        {
            var exceptionMessage = ExceptionMessage.BotNotFound(token);
            throw new Exception(exceptionMessage);
        }

        return botId;
    }

    private async Task<Guid?> GetUserId(long chatId, CancellationToken cancellationToken = default)
    {
        var userId = await _context.TelegramUsers
            .AsNoTracking()
            .Where(x => x.ChatId == chatId)
            .Select(x => x.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (userId == Guid.Empty)
        {
            return null;
        }

        return userId;
    }

    private async Task<Guid?> GetChannelId(long chatId, CancellationToken cancellationToken = default)
    {
        var userId = await _context.TelegramChannels
            .AsNoTracking()
            .Where(x => x.ChatId == chatId)
            .Select(x => x.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (userId == Guid.Empty)
        {
            return null;
        }

        return userId;
    }

    private TelegramBotClient GetClient(string token) => new(token, _httpClient);
}