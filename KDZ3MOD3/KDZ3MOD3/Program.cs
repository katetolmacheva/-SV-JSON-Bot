using Telegram.Bot;
// Tolmacheva BPI2310 Variant 12.
namespace KDZ3MOD3
{

    public static class Program
    {
        /// <summary>
        /// Launching the program.
        /// </summary>
        public static void Main()
        {
            // API of the bot.
            var user = new TelegramBotClient("6635298946:AAGvRh_vCPq9vPqUusISkOLPcCP8XNRUaB8");
            user.StartReceiving(BotMethods.HandleUpdateAsync, BotMethods.HandlePollingErrorAsync);
            Console.ReadLine();
        }
    }
}
