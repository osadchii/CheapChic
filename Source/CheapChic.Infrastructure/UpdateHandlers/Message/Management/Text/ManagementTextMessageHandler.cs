using CheapChic.Data;
using CheapChic.Infrastructure.Bot;
using CheapChic.Infrastructure.Bot.Menus;
using CheapChic.Infrastructure.Bot.Requests;
using CheapChic.Infrastructure.Services.UserService;
using Microsoft.EntityFrameworkCore;

namespace CheapChic.Infrastructure.UpdateHandlers.Message.Management.Text;

public class ManagementTextHandler : IManagementTextMessageHandler
{
    private readonly ITelegramBot _telegramBot;
    private readonly CheapChicContext _context;
    private readonly IUserService _userService;

    public ManagementTextHandler(ITelegramBot telegramBot, CheapChicContext context, IUserService userService)
    {
        _telegramBot = telegramBot;
        _context = context;
        _userService = userService;
    }

    public async Task HandleTextMessage(string token, Telegram.Bot.Types.Message message, CancellationToken cancellationToken = default)
    {
        var chatId = message.Chat.Id;
        var text = message.Text!;

        var user = await _context.TelegramUsers
            .AsNoTracking()
            .Where(x => !x.Disabled)
            .Where(x => x.ChatId == chatId)
            .FirstOrDefaultAsync(cancellationToken);

        if (user is null)
        {
            return;
        }

        var userState = await _userService.GetUserState(user.Id, cancellationToken);
        

        await _telegramBot.SendText(token,
            SendTextMessageRequest.Create(chatId, $"Your said: {text}"), cancellationToken);

        await _telegramBot.SendReplyKeyboard(token,
            SendReplyKeyboardRequest.Create(chatId, $"Your said: {text}", ConstantMenu.Management.ManagementMainMenu),
            cancellationToken);
    }
}