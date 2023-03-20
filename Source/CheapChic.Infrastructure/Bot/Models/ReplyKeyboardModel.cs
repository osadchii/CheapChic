using Telegram.Bot.Types.ReplyMarkups;

namespace CheapChic.Infrastructure.Bot.Models;

public class ReplyKeyboardModel
{
    public List<ReplyKeyboardRowModel> Rows { get; set; } = new();

    public IEnumerable<IEnumerable<KeyboardButton>> GetButtons() =>
        Rows.Select(row => row.Buttons.Select(button => new KeyboardButton(button)));
}