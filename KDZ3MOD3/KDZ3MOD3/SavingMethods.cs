using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace KDZ3MOD3;

public static class SavingMethods
{
    /// <summary>
    /// A method that allows you to choose in which format to save the file.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="botClient"></param>
    /// <param name="currentClient"></param>
    /// <param name="token"></param>
    internal static async void SaveDataOpenMenu(Message message, ITelegramBotClient botClient, User currentClient,
        CancellationToken token)
    {
        var keyboard = new ReplyKeyboardMarkup
        (
            new[]
            {
                new[]
                {
                    new KeyboardButton("1. CSV"),
                    new KeyboardButton("2. JSON")
                }
            }
        );
        await botClient.SendTextMessageAsync(message.Chat.Id,
            "Теперь выберите один из форматов файла (отправьте номер нужного пункта или нажмите кнопку):\n" +
            "1. CSV\n2. JSON", 0, ParseMode.Html, null, false,
            false, false, null, false, keyboard, token);
        currentClient.State = UserState.ChoosingSavingMethod;
    }

    /// <summary>
    /// A method that saves a file in the selected format and notifies the user about it.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="botClient"></param>
    /// <param name="currentClient"></param>
    internal static async void SaveData(Message message, ITelegramBotClient botClient, User currentClient)
    {
        var correctInput = new string[] { "1", "2", "1. CSV", "2. JSON" };
        if (correctInput.Contains(message.Text))
        {
            bool flag = message.Text == "1" || message.Text == "1. CSV";
            if (flag)
            {
                var processor = new CSVProcessing();
                await botClient.SendDocumentAsync(message.Chat.Id, InputFile.FromStream(processor.Write(currentClient.Monuments!), "result.csv"));
            }
            else
            {
                var processor = new JSONProcessing();
                await botClient.SendDocumentAsync(message.Chat.Id, InputFile.FromStream(processor.Write(currentClient.Monuments!), "result.json"));

            }
            await botClient.SendTextMessageAsync(message.Chat.Id, "Файл отправлен! Теперь для начала работы со следующим файлом пришлите его (CSV/JSON)");
            currentClient.State = UserState.Introduction;
        }
        else
        {
            await botClient.SendTextMessageAsync(message.Chat.Id, "Некорректный ввод, повторите попытку!");
        }
    }
}