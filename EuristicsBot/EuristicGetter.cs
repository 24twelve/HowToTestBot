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

    public static class EuristicGetterFields
    {
        public static List<string> EuristicsFields = new List<string>();
        private static readonly Random Rand = new Random();

        public static string GetRandomEuristicFields()
        {
            return EuristicsFields[Rand.Next(0, EuristicsFields.Count - 1)];
        }
    }
}
