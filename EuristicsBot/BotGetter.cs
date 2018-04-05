using System;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;

namespace EuristicsBot
{
    public static class BotGetter
    {
        public static TelegramBotClient Bot;

        public static TelegramBotClient GetBot(string apiKey)
        {
            Bot = new TelegramBotClient(apiKey);
            Bot.OnMessage += BotOnMessageReceived;
            Bot.OnMessageEdited += BotOnMessageReceived;
            Bot.OnReceiveError += BotOnReceiveError;

            return Bot;
        }

        private static void BotOnReceiveError(object sender, ReceiveErrorEventArgs receiveErrorEventArgs)
        {
            Logger.Log.Error(receiveErrorEventArgs.ApiRequestException);
        }


        private static async void BotOnMessageReceived(object sender, MessageEventArgs messageEventArgs)
        {
            var message = messageEventArgs.Message;
            var id = Guid.NewGuid().ToString().Substring(0, 5);
            Logger.Log.Info($"[{id}] Recieved: {message.Text}");
            if (message.Type != MessageType.TextMessage) return;

            if (message.Text.ToLowerInvariant().Contains("кто") && message.Text.ToLowerInvariant().Contains("я"))
            {
                var newJobTitle = JobTitleGetter.GetRandomJobTitle(3);
                Logger.Log.Info($"[{id}] Answered: {newJobTitle}");
                await Bot.SendTextMessageAsync(message.Chat.Id, $"Сегодня ты: {newJobTitle}.");
            }
            else if (message.Text.StartsWith("/howtotest") ||
                     message.Text.ToLowerInvariant().Contains("как") &&
                     message.Text.ToLowerInvariant().Contains("тест"))
            {
                var nextEuristic = EuristicGetter.GetRandomEuristic();
                Logger.Log.Info($"[{id}] Answered: {nextEuristic}");
                await Bot.SendTextMessageAsync(message.Chat.Id, nextEuristic);
            }
            else
            {
                await Bot.SendTextMessageAsync(message.Chat.Id,
                    "Привет. Я умею:\r\n " +
                    "1.Отвечать на вопрос: \"Как тестировать <подставь сюда что угодно>. \r\n" +
                    "2.Придумать должности для визиток. Спроси меня: \"Кто я сегодня?\"" +
                    "\r\n\r\nПуллреквесты можно слать в https://github.com/24twelve/HowToTestBot.",
                    disableWebPagePreview: true);
            }
        }
    }
}