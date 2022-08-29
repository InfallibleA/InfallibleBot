using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

var botClient = new TelegramBotClient("5443542517:AAHu17EY5R92Xnq4qMVpSmcWIRz9BuSP8tA"); //токен

using var cts = new CancellationTokenSource();
Message sentMessage;
var receiverOptions = new ReceiverOptions
{
    AllowedUpdates = Array.Empty<UpdateType>()
};
botClient.StartReceiving(
    updateHandler: HandleUpdateAsync,
    pollingErrorHandler: HandlePollingErrorAsync,
    receiverOptions: receiverOptions,
    cancellationToken: cts.Token
);

var me = await botClient.GetMeAsync();

Console.WriteLine($"Start listening for @{me.Username}");
Console.ReadLine();

cts.Cancel();

async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
{
    if (update.Message is not { } message)
        return;
    if (message.Text is not { } messageText)
        return;

    var chatId = message.Chat.Id;

    Console.WriteLine($"Received a '{messageText}' message in chat {chatId}.");

    ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]
    {
        new KeyboardButton[] { "Артур", "Курс", "Кто ты", "/help" }, 
    })
    {
        ResizeKeyboard = true
    };
    

    if (messageText == "Артур") 
    {
    sentMessage = await botClient.SendTextMessageAsync(
    chatId: chatId,
    text: "Да кто такой этот Артур, блять",
    cancellationToken: cancellationToken);
    }

    if (messageText == "$Курс€")
    {
        sentMessage = await botClient.SendTextMessageAsync(
        chatId: chatId,
        text: "Да-да я сам не в курсе, программист пока непонятно чем занят, не написал он эту часть",
        cancellationToken: cancellationToken);
    }

    if (messageText == "/help")
    {
        sentMessage = await botClient.SendTextMessageAsync(
        chatId: chatId,
        text: "Команды бота:\n" +
        "кто ты\n" +
        "Артур\n" +
        "Курс\n",
        cancellationToken: cancellationToken);
    }

    if (messageText == "Кто ты")
    {
        sentMessage = await botClient.SendTextMessageAsync(
        chatId: chatId,
        text: "Да я ботяра, тупо пажилой жмых, пажилая чим чима, тупо крипочек",
        cancellationToken: cancellationToken);
    }

    if (messageText != "/help" && messageText != "Курс" && messageText != "Артур" && messageText != "Кто ты")
    {
        sentMessage = await botClient.SendTextMessageAsync(
        chatId: chatId,
        text: "нормально разговаривай",
        cancellationToken: cancellationToken);
    }

    sentMessage = await botClient.SendTextMessageAsync(
    chatId: chatId,
    text: "Шо тебе надо?",
    replyMarkup: replyKeyboardMarkup,
    cancellationToken: cancellationToken);

}

Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
{
    var ErrorMessage = exception switch
    {
        ApiRequestException apiRequestException
            => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
        _ => exception.ToString()
    };

    Console.WriteLine(ErrorMessage);
    return Task.CompletedTask;
}