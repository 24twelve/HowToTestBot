using System;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;

namespace EuristicsBot
{
    public static class BotGetter
    {
        public static TelegramBotClient Bot;
        private static string apiKey = "586408269:AAEUBwecgTFiBaYgWjsjy83sIcRyttY-tNA";

        public static TelegramBotClient GetBot()
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
                var newJobTitle = JobTitleGetter.GetRandomJobTitle();
                Logger.Log.Info($"[{id}] Answered: {newJobTitle}");
                await Bot.SendTextMessageAsync(message.Chat.Id, $"Сегодня ты: {newJobTitle}.");
            }
            else if (message.Text.StartsWith("/idea") ||
                     message.Text.ToLowerInvariant().Contains("идея"))
            {
                var nextEuristic = EuristicGetter.GetRandomEuristic();
                Logger.Log.Info($"[{id}] Answered: {nextEuristic}");
                await Bot.SendTextMessageAsync(message.Chat.Id, nextEuristic);
            }
            else if (message.Text.StartsWith("/howtotestall") ||
                     message.Text.ToLowerInvariant().Contains("как") &&
                     message.Text.ToLowerInvariant().Contains("тестировать") &&
                     message.Text.ToLowerInvariant().Contains("все"))
            {
                var nextEuristic = EuristicGetter.GetRandomEuristic();
                Logger.Log.Info($"[{id}] Answered: {nextEuristic}");
                await Bot.SendTextMessageAsync(message.Chat.Id, nextEuristic);
            }
            else if (message.Text.StartsWith("/howtotestfields") ||
                     message.Text.ToLowerInvariant().Contains("как") &&
                     message.Text.ToLowerInvariant().Contains("тестировать") &&
                     message.Text.ToLowerInvariant().Contains("поля"))
            {
                var nextEuristicFields = EuristicGetterFields.GetRandomEuristicFields();
                Logger.Log.Info($"[{id}] Answered: {nextEuristicFields}");
                await Bot.SendTextMessageAsync(message.Chat.Id, nextEuristicFields);
            }
            else
            {
                await Bot.SendTextMessageAsync(message.Chat.Id,
                    "Привет. Я умею:\r\n " +
                    "1. Отвечать на вопрос: \"Как тестировать <все> или <поля ввода>? \r\n" +
                    "2. Придумывать должности для визиток. Спроси меня: \"Кто я сегодня? \r\n" +
                    "3. Придумывать идеи для тестирования, просто введи слово \"идея\"." +
                    "\r\n\r\nПуллреквесты можно слать в https://github.com/24twelve/HowToTestBot.",
                    disableWebPagePreview: true);
            }
        }
    }
}