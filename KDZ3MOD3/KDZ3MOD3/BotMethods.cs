using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Microsoft.VisualBasic;

namespace KDZ3MOD3;

public static class BotMethods
{
    private static List<User> _botClients = new();

    /// <summary>
    /// The method takes the user to the next menu depending on the selection.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="botClient"></param>
    /// <param name="currentClient"></param>
    /// <param name="token"></param>
    private static async void SelectOption(Message message, ITelegramBotClient botClient, User currentClient, CancellationToken token)
    {
        if (message?.Text != null)

        {
            switch (message?.Text)
            {
                case "1" or "1. Выборка по полю":
                    currentClient.State = UserState.Selecting;
                    SelectionMethods.SelectDataOpenMenu(botClient, message, token, currentClient);
                    break;
                case "2" or "2. Сортировка по полю":
                    currentClient.State = UserState.SortingMenu;
                    SortingMethods.SortDataOpenMenu(currentClient, botClient, message, token);
                    break;
                case "3" or "3. Скачать файл":
                    currentClient.State = UserState.SavingMenu;
                    SavingMethods.SaveDataOpenMenu(message, botClient, currentClient, token);
                    break;
                default:
                    await botClient.SendTextMessageAsync(message!.Chat.Id,
                    "Некорректный ввод, повторите попытку!"); break;
            }
        }
    }

    /// <summary>
    /// The method that opens the main menu.
    /// </summary>
    /// <param name="botClient"></param>
    /// <param name="message"></param>
    /// <param name="token"></param>
    internal static async void SendMainMenu(ITelegramBotClient botClient, Message message, CancellationToken token)
    {
        var keyboard = new ReplyKeyboardMarkup
        (
            new[]
            {
                new[]
                {
                    new KeyboardButton("1. Выборка по полю"),
                    new KeyboardButton("2. Сортировка по полю"),
                    new KeyboardButton("3. Скачать файл")
                }
            }
        );
        await botClient.SendTextMessageAsync(message.Chat.Id,
            "Теперь выберите один из пунктов меню (отправьте номер нужного пункта или нажмите кнопку):\n" +
            "1. Произвести выборку по одному из полей\n2. Произвести сортировку по одному из полей\n" +
            "3. Скачать обработанный файл в формате CSV или JSON", 0, ParseMode.Html,
            null, false, false, false,
            null, false, keyboard, token);
    }

    /// <summary>
    /// A method that receives a file and processes it depending on the format.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="botClient"></param>
    /// <param name="token"></param>
    /// <param name="currentClient"></param>
    private static async void ReceiveDocument(Message message, ITelegramBotClient botClient, CancellationToken token, User currentClient)
    {
        if (message?.Document != null)
        {
            var fileId = message.Document.FileId;
            var fileInfo = await botClient.GetFileAsync(fileId, token);
            var filePath = fileInfo.FilePath;
            var info = new FileInfo(filePath!);
            if (info.Extension == ".csv" || info.Extension == ".json")
            {
                var destinationFilePath = "../../../../KDZ3MOD3/botFile" + info.Extension;
                await using (Stream fileStream = System.IO.File.Create(destinationFilePath))
                {
                    await botClient.DownloadFileAsync(
                        filePath: filePath!,
                        destination: fileStream,
                        cancellationToken: token);
                }
                await botClient.SendTextMessageAsync(message.Chat.Id, "Файл успешно загружен!");

                using (var file = new StreamReader("../../../../KDZ3MOD3/botFile" + info.Extension))
                {
                    if (info.Extension == ".csv")
                    {
                        var processor = new CSVProcessing();
                        currentClient.Monuments = processor.Read(file);
                    }
                    else
                    {
                        var processor = new JSONProcessing();
                        currentClient.Monuments = processor.Read(file);
                    }
                }
                SendMainMenu(botClient, message, token);
                currentClient.State = UserState.Menu;
            }
            else
            {
                await botClient.SendTextMessageAsync(message.Chat.Id, "Некорректный формат файла, повторите попытку!");
            }
        }
    }

    /// <summary>
    /// The method that launches the bot and processes the user's messages.
    /// </summary>
    /// <param name="botClient"></param>
    /// <param name="update"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    internal static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken token)
    {
        var message = update.Message;
        if (message != null && message.Text == "/start")
        {
            _botClients = (from client in _botClients where client.Id != message.Chat.Id select client).ToList();
            await botClient.SendStickerAsync(
                chatId: message.Chat.Id,
                sticker: InputFile.FromFileId("CAACAgIAAxkBAAEEMjhl-4dKpfjrXA72BpGVCv7dhq41JAACDhkAAgwZKUnbw08zPmS7rTQE"),
                cancellationToken: token);
            await botClient.SendTextMessageAsync(message.Chat.Id, "Приветствую!\nДля начала работы отправьте файл корректного формата (CSV/JSON)");
            _botClients.Add(new User(message.Chat.Id));
        }
        else if (_botClients.ConvertAll(x => x.Id).Contains(message!.Chat.Id))
        {
            var currentUser = _botClients.Find(x => x.Id == message.Chat.Id);
            switch (currentUser!.State)
            {
                case UserState.Introduction: ReceiveDocument(message, botClient, token, currentUser); break;
                case UserState.Menu: SelectOption(message, botClient, currentUser, token); break;
                case UserState.ChoosingSelectionField: SelectionMethods.ChooseSelectionField(message, botClient, currentUser); break;
                case UserState.Selecting: SelectionMethods.SelectDataOpenMenu(botClient, message, token, currentUser); break;
                case UserState.InputtingSelectionValue: SelectionMethods.SelectData(currentUser, message, botClient, token); break;
                case UserState.Saving: SavingMethods.SaveDataOpenMenu(message, botClient, currentUser, token); break;
                case UserState.ChoosingSavingMethod: SavingMethods.SaveData(message, botClient, currentUser); break;
                case UserState.ChoosingSortingOrder: SortingMethods.SortData(message, botClient, currentUser, token); break;
            }
        }
        else
        {
            await botClient.SendTextMessageAsync(message.Chat.Id, "Команда не распознана, чтобы начать работу, введите /start");
        }
    }

    /// <summary>
    /// A method that handles query errors.
    /// </summary>
    /// <param name="botClient"></param>
    /// <param name="exception"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    internal static Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken token)
    {
        var errorMessage = exception switch
        {
            ApiRequestException apiRequestException
                => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => exception.ToString()
        };
        Console.WriteLine(errorMessage);
        return Task.CompletedTask;
    }
}