using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBotExperiments
{

    class Program
    {
        static ITelegramBotClient bot = new TelegramBotClient("6271566433:AAE_b2Tawkv_LWgVPF_vKhfPAnILQxMEKrw");
        public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Telegram.Bot.Types.Update update, CancellationToken cancellationToken)
        {

            var class1 = new Class1();



            // Некоторые действия
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(update));
            if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message)
            {
                var buttons_message = new List<KeyboardButton[]>();
                var message = update.Message;
              

                if (message.Text != null) // проверяем, является ли сообщение текстом
                {
                    if (message.Text.ToLower() == "/start")
                    {

                        // Создаем клавиатуру с меню
                        var keyboard = new ReplyKeyboardMarkup(new[]
                        {
                new[]
                {
                    new KeyboardButton("Кнопка 1"),
                    new KeyboardButton("Кнопка 2")
                }

            });
                        keyboard.OneTimeKeyboard = true;
                        keyboard.ResizeKeyboard = true;


                        await botClient.SendTextMessageAsync(message.Chat, "Добро пожаловать на борт, добрый путник!"/*, replyMarkup: keyboard*/);
                        return;
                    }

                    else if (message.Text == "Кнопка 1")
                    {
                        await botClient.SendTextMessageAsync(message.Chat, "Выполняю Команду 1!");
                    }
                    else if (message.Text == "Кнопка 2")
                    {
                        await botClient.SendTextMessageAsync(message.Chat, "Выполняю Команду 2!");
                    }

                    string message_from_user = message.Text;
                    class1.Second(message_from_user).Wait();
                    string allNumbers = string.Join(", ", Class1.matches);
                    
                    
                       // for (int i = 0; i < 10; i++)
                     //   {
                       
                     //       buttons_message.Add(new[] { new KeyboardButton(Class1.matches[i]) });
                        
                  //  }
                   // var keyboard_message = new ReplyKeyboardMarkup(buttons_message.ToArray());
                    // keyboard_message.OneTimeKeyboard = true;
                    //keyboard_message.ResizeKeyboard = true;

                    await botClient.SendTextMessageAsync(message.Chat, "Вот домены: " + allNumbers/*, replyMarkup: keyboard_message*/);
                    Class1.matches.Clear();
                }
            }
        }

        public static async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            // Некоторые действия
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(exception));
        }


        static void Main(string[] args)
        {

            Console.WriteLine("Запущен бот " + bot.GetMeAsync().Result.FirstName);

            var cts = new CancellationTokenSource();
            var cancellationToken = cts.Token;
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = { }, // receive all update types
            };
            bot.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                receiverOptions,
                cancellationToken
            );
            Console.ReadLine();
        }

        
    }

    
}