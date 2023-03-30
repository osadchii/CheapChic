namespace CheapChic.Infrastructure.Bot.Models;

public class ReplyKeyboardBuilder
{
    private readonly ReplyKeyboardModel _keyboard;
    private ReplyKeyboardRowModel _currentRow;

    private ReplyKeyboardBuilder()
    {
        _keyboard = new ReplyKeyboardModel();
    }

    public static ReplyKeyboardBuilder Create()
    {
        return new ReplyKeyboardBuilder();
    }

    public ReplyKeyboardBuilder AddRow()
    {
        var row = new ReplyKeyboardRowModel();
        
        _currentRow = row;
        _keyboard.Rows.Add(row);

        return this;
    }

    public ReplyKeyboardBuilder AddButton(string text)
    {
        _currentRow.Buttons.Add(text);
        
        return this;
    }

    public ReplyKeyboardModel Build() => _keyboard;
}