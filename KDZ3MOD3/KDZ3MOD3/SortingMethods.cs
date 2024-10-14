using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;


namespace KDZ3MOD3;

public static class SortingMethods
{
    /// <summary>
    /// A method that processes the user's choice.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="botClient"></param>
    /// <param name="currentClient"></param>
    /// <param name="token"></param>
    internal static async void SortData(Message message, ITelegramBotClient botClient, User currentClient,
        CancellationToken token)
    {
        var correctInput = new string[] { "1", "2", "1. SculpName в прямом алфавитном порядке", "2. ManufactYear по убыванию" };
        if (correctInput.Contains(message.Text))
        {
            bool flag = message.Text == "1" || message.Text == "1. SculpName в прямом алфавитном порядке";
            DataProcessing.Sort(currentClient, flag);
            await botClient.SendTextMessageAsync(message.Chat.Id,"Сортировка прошла успешно!");
            BotMethods.SendMainMenu(botClient, message, token);
            currentClient.State = UserState.Menu;
        }
        else
        {
            await botClient.SendTextMessageAsync(message.Chat.Id,"Некорректный ввод, повторите попытку!");
        }
    }

    /// <summary>
    /// A method that outputs which fields can be sorted.
    /// </summary>
    /// <param name="currentClient"></param>
    /// <param name="botClient"></param>
    /// <param name="message"></param>
    /// <param name="token"></param>
    internal static async void SortDataOpenMenu(User currentClient, ITelegramBotClient botClient,
        Message message, CancellationToken token)
    {
        var keyboard = new ReplyKeyboardMarkup
        (
            new[]
            {
                new[]
                {
                    new KeyboardButton("1. SculpName в прямом алфавитном порядке"),
                    new KeyboardButton("2. ManufactYear по убыванию")
                }
            }
        );
        await botClient.SendTextMessageAsync(message.Chat.Id,
            "Теперь выберите один из способов сортировки (отправьте номер нужного пункта или нажмите кнопку):\n" +
            "1. SculpName по алфавиту в прямом порядке\n2. ManufactYear по убыванию", 0, ParseMode.Html, null, false, false, false, null, false, keyboard, token);
        currentClient.State = UserState.ChoosingSortingOrder;
    }
}