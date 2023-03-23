using CheapChic.Data.Constants;

namespace CheapChic.Infrastructure.Bot.Constants;

public static class MessageText
{
    public static class Management
    {
        public static class Common
        {
            public const string SelectAMenuItem = "Выберите пункт меню";
        }
        
        public static class AddBot
        {
            public const string SendToken = "Отправьте токен вашего бота. Для получения токена вашего бота воспользуйтесь официальным Телеграм-ботом @BotFather";

            public const string SendName =
                $"Отправьте название вашего бота. Максимум 16 символов";

            public const string InvalidToken =
                "Неверный токен. Для получения токена вашего бота воспользуйтесь официальным Телеграм-ботом @BotFather";

            public const string TokenAlreadyExists =
                "Бот с этим токеном уже добавлен. Для получения токена вашего бота воспользуйтесь официальным Телеграм-ботом @BotFather";

            public const string NameAlreadyExists =
                "Бот с этим названием у вас уже есть. Добавьте новое уникальное название";

            public const string NameIsTooLong = "Слишком длинное название бота. Максимальная длина 16 символов.";

            public const string SuccessfulAdded = "Бот успешно добавлен.";
        }
    }
}