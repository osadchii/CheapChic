using CheapChic.Data;
using CheapChic.Data.Entities;
using CheapChic.Infrastructure.Bot;
using CheapChic.Infrastructure.Bot.Constants;
using CheapChic.Infrastructure.Bot.Requests;
using CheapChic.Infrastructure.Configuration.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace CheapChic.Infrastructure.UpdateHandlers.MyChatMember;

public class MyChatMemberHandler : IMyChatMemberHandler
{
    private readonly CheapChicContext _context;
    private readonly ITelegramBot _telegramBot;
    private readonly IOptions<ManagementBotOptions> _managementBotOptions;

    public MyChatMemberHandler(CheapChicContext context, ITelegramBot telegramBot,
        IOptions<ManagementBotOptions> managementBotOptions)
    {
        _context = context;
        _telegramBot = telegramBot;
        _managementBotOptions = managementBotOptions;
    }

    public async Task HandleMyChatMember(string token, ChatMemberUpdated myChatMember,
        CancellationToken cancellationToken = default)
    {
        var channelChatId = myChatMember.Chat.Id;
        var channelTitle = myChatMember.Chat.Title;
        var userChatId = myChatMember.From.Id;
        var status = myChatMember.NewChatMember.Status;

        var channel = await EnsureChannelCreated(channelChatId, channelTitle, cancellationToken);

        var telegramBot = await _context.TelegramBots
            .AsNoTracking()
            .Include(x => x.Owner)
            .Where(x => x.Token == token)
            .Where(x => x.Owner.ChatId == userChatId)
            .FirstOrDefaultAsync(cancellationToken);

        if (telegramBot is null)
        {
            return;
        }

        var mapping = await _context.TelegramBotChannelMappings
            .Where(x => x.BotId == telegramBot.Id)
            .Where(x => x.ChannelId == channel.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (status == ChatMemberStatus.Administrator)
        {
            if (mapping is not null)
            {
                return;
            }

            mapping = new TelegramBotChannelMappingEntity
            {
                ChannelId = channel.Id,
                BotId = telegramBot.Id
            };

            await _context.AddAsync(mapping, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            var channelAddedRequest = SendTextMessageRequest.Create(telegramBot.Owner.ChatId,
                MessageText.Management.AddBot.BotAddedToChannel(telegramBot.Name, channelTitle));
            await _telegramBot.SendText(_managementBotOptions.Value.Token, channelAddedRequest, cancellationToken);
        }
        else
        {
            if (mapping is null)
            {
                return;
            }

            _context.TelegramBotChannelMappings
                .Remove(mapping);
            await _context.SaveChangesAsync(cancellationToken);

            var channelRemovedRequest = SendTextMessageRequest.Create(telegramBot.Owner.ChatId,
                MessageText.Management.AddBot.BotRemovedFromChannel(telegramBot.Name, channelTitle));
            await _telegramBot.SendText(_managementBotOptions.Value.Token, channelRemovedRequest, cancellationToken);
        }
    }

    private async Task<TelegramChannelEntity> EnsureChannelCreated(long channelChatId, string channelTitle,
        CancellationToken cancellationToken)
    {
        var channel = await _context.TelegramChannels
            .FirstOrDefaultAsync(x => x.ChatId == channelChatId, cancellationToken);

        if (channel is null)
        {
            channel = new TelegramChannelEntity
            {
                ChatId = channelChatId,
                ChannelName = channelTitle,
            };

            await _context.AddAsync(channel, cancellationToken);
        }
        else
        {
            channel.ChannelName = channelTitle;
        }

        await _context.SaveChangesAsync(cancellationToken);
        return channel;
    }
}