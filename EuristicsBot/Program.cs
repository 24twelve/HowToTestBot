using System;
using System.IO;
using System.Threading;
using File = System.IO.File;

namespace EuristicsBot
{
	class Program
	{
		//todo: external hosting and deploy and subscription+
		//todo: inline queries
		//todo: better hide api key
		//todo: tests and proper architecture 
		//todo: fill with euristics
		//todo: user friendlyness and ux
		//todo: remove memory leaks without restarting

		static void Main(string[] args)
		{
			Logger.InitLogger();
			try
			{
				Init();
			}
			catch (Exception e)
			{
				Logger.Log.Error($"Exception during init! It was {e}");
			}
			Logger.Log.Info("=============[ Started bot ]=============");
			while (true)
			{
				Thread.Sleep(TimeSpan.FromMinutes(10));
				Logger.Log.Info("Still working...");
			}
		}

		public static void Init()
		{
			apiKey = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "apiKey"));
			var euristics = File.ReadAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Euristics.txt"));
			foreach (var euristic in euristics)
			{
				EuristicGetter.Euristics.Add(euristic);
			}
			var bot = BotGetter.GetBot(apiKey);
			bot.StartReceiving();
		}

		private static string apiKey;
	}
}