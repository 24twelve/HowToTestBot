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
			Logger.Log.Info($"Recieved: {message.Text}");
			if (message == null || message.Type != MessageType.TextMessage) return;

			if (message.Text.StartsWith("/howtotest") ||
			    message.Text.ToLower().Contains("как") && message.Text.ToLower().Contains("тест"))
			{
				var nextEuristic = EuristicGetter.GetRandomEuristic();
				await Bot.SendTextMessageAsync(message.Chat.Id, nextEuristic);
			}
			else
			{
				await Bot.SendTextMessageAsync(message.Chat.Id,
					"Спроси меня: \"Как тестировать <подставь название>?\".\r\n\r\nПуллреквесты можно слать в https://github.com/24twelve/HowToTestBot.",
					disableWebPagePreview: true);
			}
		}
	}
}