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

            public const string NameIsTooLong = "Слишком длинное название бота. Максимальная длина 16 символов";

            public const string SuccessfulAdded = "Бот успешно добавлен. Теперь добавьте бот на канал для публикации объявлений и сделайте его администратором";

            public static string BotAddedToChannel(string botName, string channelTitle) =>
                $"Бот <b>{botName}</b> был добавлен на канал <b>{channelTitle}</b>. Объявления будут публиковаться на этот канал";
            public static string BotRemovedFromChannel(string botName, string channelTitle) =>
                $"Бот <b>{botName}</b> был удален с канала <b>{channelTitle}</b>. Объявления не будут публиковаться на этот канал";
        }
        
        public static class MyBots
        {
            public static string BotWithNameNotFound(string name) => $"Телеграм бот с именем {name} не найден";
            public const string SendCurrency = "Пришлите название валюты вашего бота (максимум 5 символов)";
            public const string SendPublishDays = "Пришлите количество дней, через которое объявление будет автоматически деактивировано (не более 28 дней)";
            public const string SendPublishEveryHours = "Пришлите количество часов, через которое объявление будет отправлено повторно (не менее 1 часа)";
        }
    }
    
    public static class Retailer
    {
        public static class Common
        {
            public const string SelectAMenuItem = "Выберите пункт меню";
        }
        
        public static class AddAd
        {
            public const string SelectAction = "Выберите действие";
            public const string SendName = "Отправьте краткое название вашего объявления (до 128 символов)";
            public const string NameIsNotUnique =
                "У вас уже есть активное объявление с таким названием. Выберите другое название или отключите ваше существующее объявление";
            public const string SendDescription = "Отправьте подробное описание вашего объявления (до 3072 символов)";
            public const string SendPrice = "Отправьте цену";
            public const string SendPhoto = "Пришлите фотографии для вашего объявления (до 4 фотографий) и нажмите \"Готово\"";
            public const string PhotoAddedToTheAd = "Фото добавлено к объявлению";
            public const string CantAddPhotoToTheAd = "Невозможно добавить это фото к объявлению. Используйте другое";
            public const string PhotoAlreadyInTheAd = "Фото уже добавлено к объявлению";
            public const string AdConfirmation = "Почти все готово! Публикуем?";
            public const string AdPublished =
                "Объявление успешно создано. Оно будет опубликовано на канале в ближайшее время";
            public static string CantAddMoreThanPhotos(int count) =>
                $"Невозможно добавить больше {count} фото к объявлению";
        }
    }
}