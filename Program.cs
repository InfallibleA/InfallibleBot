using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;


var botClient = new TelegramBotClient("5443542517:AAEvPuC_qRra_Wh7Th-VjV5L2ZbJ8Q1ekuo");

using var cts = new CancellationTokenSource();
Message sentMessage;
// StartReceiving does not block the caller thread. Receiving is done on the ThreadPool.
var receiverOptions = new ReceiverOptions
{
    AllowedUpdates = Array.Empty<UpdateType>() // receive all update types
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

// Send cancellation request to stop bot
cts.Cancel();

async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
{
    // Only process Message updates: https://core.telegram.org/bots/api#message
    if (update.Message is not { } message)
        return;
    // Only process text messages
    if (message.Text is not { } messageText)
        return;

    var chatId = message.Chat.Id;

    Console.WriteLine($"Received a '{messageText}' message in chat {chatId}.");

    // Echo received message text

    if (messageText == "Артур") 
    {
    sentMessage = await botClient.SendTextMessageAsync(
    chatId: chatId,
    text: "Да кто такой этот Артур",
    cancellationToken: cancellationToken);
    }

    if (messageText == "Стикер")
    {
        sentMessage = await botClient.SendTextMessageAsync(
        chatId: chatId,
        photo: "https://github.com/TelegramBots/book/raw/master/src/docs/photo-ara.jpg",
    caption: "<b>Ara bird</b>. <i>Source</i>: <a href=\"https://pixabay.com\">Pixabay</a>",
    parseMode: ParseMode.Html,
        cancellationToken: cancellationToken);
    }
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