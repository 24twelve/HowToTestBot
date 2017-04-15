using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.InputMessageContents;

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
			Bot.OnInlineQuery += BotOnInlineQueryReceived;

			return Bot;
		}

		private static void BotOnReceiveError(object sender, ReceiveErrorEventArgs receiveErrorEventArgs)
		{
			Logger.Log.Error(receiveErrorEventArgs.ApiRequestException);
		}


		private static async void BotOnMessageReceived(object sender, MessageEventArgs messageEventArgs)
		{
			var message = messageEventArgs.Message;
			Logger.Log.Info($"Recieved: {message}");
			if (message == null || message.Type != MessageType.TextMessage) return;

			if (message.Text.ToLower().Contains("как") && message.Text.ToLower().Contains("тест"))
			{
				var nextEuristic = EuristicGetter.GetRandomEuristic();
				await Bot.SendTextMessageAsync(message.Chat.Id, nextEuristic);
			}
			else
			{
				await Bot.SendTextMessageAsync(message.Chat.Id,
					"Если нет идей, как протестировать приложение — спроси меня, \"Как протестировать %имя релиза%?\". Пуллреквесты можно слать в https://github.com/24twelve/HowToTestBot.");
				//todo buttons
			}
		}

		private static async void BotOnInlineQueryReceived(object sender, InlineQueryEventArgs inlineQueryEventArgs)
		{
			var recievedMessage = inlineQueryEventArgs.InlineQuery.Query;
			Logger.Log.Info($"Recieved: {recievedMessage}");
			string answer;
			if (recievedMessage.ToLower().Contains("как") && recievedMessage.ToLower().Contains("тест"))
			{
				answer = EuristicGetter.GetRandomEuristic();
			}
			else
			{
				answer = "Спроси меня, как протестировать что-то.";
			}

			InlineQueryResult[] results =
			{
				new InlineQueryResultNew
				{
					InputMessageContent = new InputTextMessageContent
					{
						MessageText = answer
					}
				}
			};

			await Bot.AnswerInlineQueryAsync(inlineQueryEventArgs.InlineQuery.Id, results, isPersonal: true,
				cacheTime: 0);
		}
	}
}