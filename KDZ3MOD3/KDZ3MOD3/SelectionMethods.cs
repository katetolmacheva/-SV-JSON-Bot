using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace KDZ3MOD3;

public static class SelectionMethods
{
    /// <summary>
    /// A method that notifies the user that the selection was successful.
    /// </summary>
    /// <param name="currentClient"></param>
    /// <param name="message"></param>
    /// <param name="botClient"></param>
    /// <param name="token"></param>
    internal static async void SelectData(User currentClient, Message message, ITelegramBotClient botClient, CancellationToken token)
    {
        if (message.Text != null)
        {
            DataProcessing.Filter(currentClient, currentClient.SelectionField!, message.Text);
            if (currentClient.Monuments!.Count == 0)
            {
                await botClient.SendTextMessageAsync(message.Chat.Id, "В результате обработки получился пустой список объектов");
            }
            await botClient.SendTextMessageAsync(message.Chat.Id, "Выборка прошла успешно!");
            BotMethods.SendMainMenu(botClient, message, token);
            currentClient.State = UserState.Menu;
        }
        else
        {
            await botClient.SendTextMessageAsync(message.Chat.Id,"Некорректный ввод, повторите попытку!");
        }
    }

    /// <summary>
    /// The method that processes the user's request from the menu.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="botClient"></param>
    /// <param name="currentClient"></param>
    /// <exception cref="ArgumentException"></exception>
    internal static async void ChooseSelectionField(Message message, ITelegramBotClient botClient, User currentClient)
    {
        var correctInput = new string[] { "1", "2", "3", "1. SculpName", "2. LocationPlace", "3. ManufactYear и Material" };
        if (correctInput.Contains(message.Text))
        {
            await botClient.SendTextMessageAsync(message.Chat.Id,"Ваш выбор обработан! Введите значение для выборки:");
            currentClient.SelectionField = message.Text switch
            {
                "1" or "1. SculpName" => "SculpName",
                "2" or "2. LocationPlace" => "LocationPlace",
                "3" or "3. ManufactYear и Material" => "ManufactYear Material",
                _ => throw new ArgumentException()
            };
            currentClient.State = UserState.InputtingSelectionValue;
        }
        else
        {
            await botClient.SendTextMessageAsync(message.Chat.Id,"Некорректный ввод, повторите попытку!");
        }
    }

    /// <summary>
    /// A method that outputs which fields can be filtered.
    /// </summary>
    /// <param name="botClient"></param>
    /// <param name="message"></param>
    /// <param name="token"></param>
    /// <param name="currentClient"></param>
    internal static async void SelectDataOpenMenu(ITelegramBotClient botClient, Message message, CancellationToken token,
        User currentClient)
    {
        var keyboard = new ReplyKeyboardMarkup
        (
            new[]
            {
                new[]
                {
                    new KeyboardButton("1. SculpName"),
                    new KeyboardButton("2. LocationPlace"),
                    new KeyboardButton("3. ManufactYear и Material")
                }
            }
        );
        await botClient.SendTextMessageAsync(message.Chat.Id,
            "Теперь выберите одно из полей для выборки (отправьте номер нужного пункта или нажмите кнопку):\n" +
            "1. SculpName\n2. LocationPlace\n3. ManufactYear и Material", 0, ParseMode.Html,
            null, false, false, false,
            null, false, keyboard, token);
        currentClient.State = UserState.ChoosingSelectionField;
    }
}