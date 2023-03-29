namespace CheapChic.Infrastructure.Bot.Constants;

public static class MenuText
{
    public static class Management
    {
        public static class Common
        {
            public const string Back = "⬅️ Назад";
        }

        public static class MainMenu
        {
            public const string MyBots = "Мои боты";
            public const string AddBot = "Добавить бота";
        }
        
        public static class MyBotsMenu
        {
            public const string Disable = "Отключить";
            public const string Enable = "Включить";
        }
    }
    
    public static class Retailer
    {
        public static class Common
        {
            public const string Back = "⬅️ Назад";
        }
        
        public static class MainMenu
        {
            public const string MyAnnouncements = "Мои объявления";
            public const string AddAnnouncement = "Добавить объявление";
        }
        
        public static class AddAdMenu
        {
            public const string Sell = "Продаю";
            public const string Buy = "Покупаю";
            public const string OfferAService = "Предлагаю услугу";
            public const string LookingForAService = "Ищу услугу";
        }
        
        public static class AddAdPriceMenu
        {
            public const string Negotiated = "Договорная";
        }
        
        public static class AddAdPhotoMenu
        {
            public const string ClearPhotos = "Удалить добавленные фотографии";
            public const string Done = "Готово";
        }
        
        public static class AddAdConfirmationMenu
        {
            public const string Publish = "Опубликовать";
        }
    }
}