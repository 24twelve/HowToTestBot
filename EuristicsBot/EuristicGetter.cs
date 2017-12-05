using System;
using System.Collections.Generic;

namespace EuristicsBot
{
	public static class EuristicGetter
	{
		public static List<string> Euristics = new List<string>();
		private static readonly Random Rand = new Random();

		public static string GetRandomEuristic()
		{
			return Euristics[Rand.Next(0, Euristics.Count-1)];
		}
	}
}
