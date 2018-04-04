using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace EuristicsBot
{
    public static class JobTitleGetter
    {
        //todo: пасхалки <0, очень большое число
        //todo: logging
        //todo: прикрутить к командам бота
        //todo: типа - кем хочешь быть сегодня

        public static string GetRandomJobTitle(int wordCount)
        {
            if (wordCount > Words.Length)
                wordCount = Words.Length;
            return GetRandomJobTitleInternal(wordCount, Words);
        }

        private static string GetRandomJobTitleInternal(int wordCount, TitlePart[] wordArray)
        {
            var result = new List<TitlePart>();
            while (result.Count <= wordCount)
            {
                var randomWord = GetRandomTitlePart(wordArray);
                result.Add(randomWord);
                wordArray = wordArray.Except(new[] {randomWord}).ToArray();
            }

            result = result.Distinct().ToList();
            var last = result.Last();

            if (last.Specificator == WordPlaceSpecificator.NotOnEnd)
            {
                var suitable = result.FirstOrDefault(x => x.Specificator == WordPlaceSpecificator.OnlyOnEnd)
                               ?? result.FirstOrDefault(x => x.Specificator == WordPlaceSpecificator.Anywhere);
                result.Add(suitable);
                result.Remove(suitable);
            }
            result.RemoveAll(x => x.Specificator == WordPlaceSpecificator.OnlyOnEnd);
            result.Add(GetRandomTitlePart(wordArray, x => x.Specificator == WordPlaceSpecificator.OnlyOnEnd));

            result = RemoveMutuallyExclusiveParts(result.ToArray()).ToList();

            return string.Join(" ", result.Select(x => x.Title).Distinct());
        }


        private static TitlePart GetRandomTitlePart(TitlePart[] array, Func<TitlePart, bool> selector)
        {
            return GetRandomTitlePart(array.Where(selector).ToArray());
        }

        private static TitlePart GetRandomTitlePart(TitlePart[] array)
        {
            var cnt = array.Length;
            if (cnt <= 1)
                return TitlePart.StubTitlePart;
            return array[Random.Next(cnt - 1)];
        }

        private static TitlePart[] RemoveMutuallyExclusiveParts(TitlePart[] array)
        {
            var newArray = array.ToArray(); //не придумал ничего лучше за минуту :)
            foreach (var titlePart in newArray)
            {
                var mutuallyExclusiveRuleSubset =
                    MutuallyExclusiveTitleParts.FirstOrDefault(x => x.Count(y => y.Title == titlePart.Title) > 0);
                if (mutuallyExclusiveRuleSubset != null)
                {
                    var newSubset = mutuallyExclusiveRuleSubset.ToArray().Where(x => x.Title != titlePart.Title);
                    newArray = newArray.Where(x => !newSubset.Contains(x)).ToArray();
                }
            }
            return newArray;
        }

        private static readonly TitlePart[][] MutuallyExclusiveTitleParts =
        {
            new[]
            {
                new TitlePart("Senior", WordPlaceSpecificator.NotOnEnd),
                new TitlePart("Junior", WordPlaceSpecificator.NotOnEnd),
                new TitlePart("Middle", WordPlaceSpecificator.NotOnEnd),
                new TitlePart("Star", WordPlaceSpecificator.NotOnEnd)
            },
            new[]
            {
                new TitlePart("Automation", WordPlaceSpecificator.NotOnEnd),
                new TitlePart("Manual", WordPlaceSpecificator.NotOnEnd)
            },
            new[]
            {
                new TitlePart("Quality Assurance", WordPlaceSpecificator.NotOnEnd),
                new TitlePart("Quality", WordPlaceSpecificator.NotOnEnd),
                new TitlePart("QA", WordPlaceSpecificator.NotOnEnd)
            },
            new[]
            {
                new TitlePart("Frontend", WordPlaceSpecificator.NotOnEnd),
                new TitlePart("Backend", WordPlaceSpecificator.NotOnEnd),
                new TitlePart("Fullstack", WordPlaceSpecificator.NotOnEnd),
            },
            new[]
            {
                new TitlePart("Specialist", WordPlaceSpecificator.Anywhere),
                new TitlePart("Tester", WordPlaceSpecificator.OnlyOnEnd),
                new TitlePart("Engineer", WordPlaceSpecificator.Anywhere),
                new TitlePart("Developer", WordPlaceSpecificator.Anywhere),
            },
            new[]
            {
                new TitlePart("Performance", WordPlaceSpecificator.NotOnEnd),
                new TitlePart("Load", WordPlaceSpecificator.NotOnEnd),
                new TitlePart("Stress", WordPlaceSpecificator.NotOnEnd),
            },
            new[]
            {
                new TitlePart("Security", WordPlaceSpecificator.NotOnEnd),
                new TitlePart("Pentesting", WordPlaceSpecificator.NotOnEnd),
                new TitlePart("White Hat", WordPlaceSpecificator.NotOnEnd),
            },
            new[]
            {
                new TitlePart("C#", WordPlaceSpecificator.Anywhere),
                new TitlePart("Python", WordPlaceSpecificator.Anywhere),
                new TitlePart("XML", WordPlaceSpecificator.Anywhere),
                new TitlePart("HTML", WordPlaceSpecificator.Anywhere),
                new TitlePart("Erlang", WordPlaceSpecificator.Anywhere),
                new TitlePart("Elixir", WordPlaceSpecificator.Anywhere),
                new TitlePart("Java", WordPlaceSpecificator.Anywhere),
                new TitlePart("Javascript", WordPlaceSpecificator.Anywhere),
                new TitlePart("1C", WordPlaceSpecificator.Anywhere),
                new TitlePart("Manual", WordPlaceSpecificator.NotOnEnd),
                new TitlePart("Haskell", WordPlaceSpecificator.Anywhere),
            },
            new[]
            {
                new TitlePart("Tester", WordPlaceSpecificator.OnlyOnEnd),
                new TitlePart("Testing", WordPlaceSpecificator.NotOnEnd),
            },
            new[]
            {
                new TitlePart("Auditor", WordPlaceSpecificator.OnlyOnEnd),
                new TitlePart("Master", WordPlaceSpecificator.NotOnEnd),
            }
        };

        private static readonly TitlePart[] Words =
        {
            new TitlePart("QA", WordPlaceSpecificator.NotOnEnd),
            new TitlePart("Tester", WordPlaceSpecificator.OnlyOnEnd),
            new TitlePart("Testing", WordPlaceSpecificator.NotOnEnd),
            new TitlePart("Coach", WordPlaceSpecificator.OnlyOnEnd),
            new TitlePart("Ad hoc", WordPlaceSpecificator.NotOnEnd),
            new TitlePart("Smoke", WordPlaceSpecificator.NotOnEnd),
            new TitlePart("Mentor", WordPlaceSpecificator.OnlyOnEnd),
            new TitlePart("Master", WordPlaceSpecificator.OnlyOnEnd),
            new TitlePart("Security", WordPlaceSpecificator.NotOnEnd),
            new TitlePart("Pentesting", WordPlaceSpecificator.NotOnEnd),
            new TitlePart("White Hat", WordPlaceSpecificator.NotOnEnd),
            new TitlePart("Tutor", WordPlaceSpecificator.OnlyOnEnd),
            new TitlePart("Specialist", WordPlaceSpecificator.OnlyOnEnd),
            new TitlePart("Agile", WordPlaceSpecificator.NotOnEnd),
            new TitlePart("Expert", WordPlaceSpecificator.Anywhere),
            new TitlePart("Fellow", WordPlaceSpecificator.Anywhere),
            new TitlePart("Accurate", WordPlaceSpecificator.NotOnEnd),
            new TitlePart("Quality Assurance", WordPlaceSpecificator.NotOnEnd),
            new TitlePart("Quality", WordPlaceSpecificator.NotOnEnd),
            new TitlePart("Automation", WordPlaceSpecificator.NotOnEnd),
            new TitlePart("Automation", WordPlaceSpecificator.NotOnEnd),
            new TitlePart("Automation", WordPlaceSpecificator.NotOnEnd),
            new TitlePart("Automation", WordPlaceSpecificator.NotOnEnd),
            new TitlePart("Automation", WordPlaceSpecificator.NotOnEnd),
            new TitlePart("Automation", WordPlaceSpecificator.NotOnEnd),
            new TitlePart("Automation", WordPlaceSpecificator.NotOnEnd),
            new TitlePart("Automation", WordPlaceSpecificator.NotOnEnd),
            new TitlePart("Automation", WordPlaceSpecificator.NotOnEnd),
            new TitlePart("Automation", WordPlaceSpecificator.NotOnEnd),
            new TitlePart("Automation", WordPlaceSpecificator.NotOnEnd),
            new TitlePart("Automation", WordPlaceSpecificator.NotOnEnd),
            new TitlePart("Automation", WordPlaceSpecificator.NotOnEnd),
            new TitlePart("Automation", WordPlaceSpecificator.NotOnEnd),
            new TitlePart("Automation", WordPlaceSpecificator.NotOnEnd),
            new TitlePart("Automation", WordPlaceSpecificator.NotOnEnd),
            new TitlePart("Automation", WordPlaceSpecificator.NotOnEnd),
            new TitlePart("Scrum", WordPlaceSpecificator.NotOnEnd),
            new TitlePart("Analyst", WordPlaceSpecificator.OnlyOnEnd),
            new TitlePart("Developer", WordPlaceSpecificator.OnlyOnEnd),
            new TitlePart("Devops", WordPlaceSpecificator.NotOnEnd),
            new TitlePart("Frontend", WordPlaceSpecificator.NotOnEnd),
            new TitlePart("Fullstack", WordPlaceSpecificator.NotOnEnd),
            new TitlePart("Backend", WordPlaceSpecificator.NotOnEnd),
            new TitlePart("Database", WordPlaceSpecificator.NotOnEnd),
            new TitlePart("Auditor", WordPlaceSpecificator.NotOnEnd),
            new TitlePart("Exploratory", WordPlaceSpecificator.NotOnEnd),
            new TitlePart("Black Box", WordPlaceSpecificator.NotOnEnd),
            new TitlePart("White Box", WordPlaceSpecificator.NotOnEnd),
            new TitlePart("Lead", WordPlaceSpecificator.Anywhere),
            new TitlePart("Teamlead", WordPlaceSpecificator.Anywhere),
            new TitlePart("Jedi", WordPlaceSpecificator.Anywhere),
            new TitlePart("CI", WordPlaceSpecificator.NotOnEnd),
            new TitlePart("Manual", WordPlaceSpecificator.NotOnEnd),
            new TitlePart("Manual", WordPlaceSpecificator.NotOnEnd),
            new TitlePart("Manual", WordPlaceSpecificator.NotOnEnd),
            new TitlePart("Manual", WordPlaceSpecificator.NotOnEnd),
            new TitlePart("Manual", WordPlaceSpecificator.NotOnEnd),
            new TitlePart("Manual", WordPlaceSpecificator.NotOnEnd),
            new TitlePart("Manual", WordPlaceSpecificator.NotOnEnd),
            new TitlePart("Manual", WordPlaceSpecificator.NotOnEnd),
            new TitlePart("Manual", WordPlaceSpecificator.NotOnEnd),
            new TitlePart("Manual", WordPlaceSpecificator.NotOnEnd),
            new TitlePart("Manual", WordPlaceSpecificator.NotOnEnd),
            new TitlePart("Manual", WordPlaceSpecificator.NotOnEnd),
            new TitlePart("Manual", WordPlaceSpecificator.NotOnEnd),
            new TitlePart("Manual", WordPlaceSpecificator.NotOnEnd),
            new TitlePart("Manual", WordPlaceSpecificator.NotOnEnd),
            new TitlePart("Manual", WordPlaceSpecificator.NotOnEnd),
            new TitlePart("Manual", WordPlaceSpecificator.NotOnEnd),
            new TitlePart("Manual", WordPlaceSpecificator.NotOnEnd),
            new TitlePart("Manual", WordPlaceSpecificator.NotOnEnd),
            new TitlePart("Manager", WordPlaceSpecificator.OnlyOnEnd),
            new TitlePart("Bigdata", WordPlaceSpecificator.NotOnEnd),
            new TitlePart("Machine Learning", WordPlaceSpecificator.NotOnEnd),
            new TitlePart("Blockchain", WordPlaceSpecificator.NotOnEnd),
            new TitlePart("Experienced", WordPlaceSpecificator.NotOnEnd),
            new TitlePart("Extreme", WordPlaceSpecificator.NotOnEnd),
            new TitlePart("Functional", WordPlaceSpecificator.NotOnEnd),
            new TitlePart("Engineer", WordPlaceSpecificator.OnlyOnEnd),
            new TitlePart("Star", WordPlaceSpecificator.NotOnEnd),
            new TitlePart("Senior", WordPlaceSpecificator.NotOnEnd),
            new TitlePart("Senior", WordPlaceSpecificator.NotOnEnd),
            new TitlePart("Senior", WordPlaceSpecificator.NotOnEnd),
            new TitlePart("Senior", WordPlaceSpecificator.NotOnEnd),
            new TitlePart("Senior", WordPlaceSpecificator.NotOnEnd),
            new TitlePart("Senior", WordPlaceSpecificator.NotOnEnd),
            new TitlePart("Senior", WordPlaceSpecificator.NotOnEnd),
            new TitlePart("Senior", WordPlaceSpecificator.NotOnEnd),
            new TitlePart("Senior", WordPlaceSpecificator.NotOnEnd),
            new TitlePart("Senior", WordPlaceSpecificator.NotOnEnd),
            new TitlePart("Senior", WordPlaceSpecificator.NotOnEnd),
            new TitlePart("Senior", WordPlaceSpecificator.NotOnEnd),
            new TitlePart("Senior", WordPlaceSpecificator.NotOnEnd),
            new TitlePart("Senior", WordPlaceSpecificator.NotOnEnd),
            new TitlePart("Senior", WordPlaceSpecificator.NotOnEnd),
            new TitlePart("Senior", WordPlaceSpecificator.NotOnEnd),
            new TitlePart("Senior", WordPlaceSpecificator.NotOnEnd),
            new TitlePart("Middle", WordPlaceSpecificator.NotOnEnd),
            new TitlePart("Middle", WordPlaceSpecificator.NotOnEnd),
            new TitlePart("Middle", WordPlaceSpecificator.NotOnEnd),
            new TitlePart("Junior", WordPlaceSpecificator.NotOnEnd),
            new TitlePart("Junior", WordPlaceSpecificator.NotOnEnd),
            new TitlePart("Junior", WordPlaceSpecificator.NotOnEnd),
            new TitlePart("Junior", WordPlaceSpecificator.NotOnEnd),
            new TitlePart("Junior", WordPlaceSpecificator.NotOnEnd),
            new TitlePart("Junior", WordPlaceSpecificator.NotOnEnd),
            new TitlePart("Junior", WordPlaceSpecificator.NotOnEnd),
            new TitlePart("Junior", WordPlaceSpecificator.NotOnEnd),
            new TitlePart("Junior", WordPlaceSpecificator.NotOnEnd),
            new TitlePart("Junior", WordPlaceSpecificator.NotOnEnd),
            new TitlePart("Junior", WordPlaceSpecificator.NotOnEnd),
            new TitlePart("Junior", WordPlaceSpecificator.NotOnEnd),
            new TitlePart("Junior", WordPlaceSpecificator.NotOnEnd),
            new TitlePart("Junior", WordPlaceSpecificator.NotOnEnd),
            new TitlePart("Junior", WordPlaceSpecificator.NotOnEnd),
            new TitlePart("Risk", WordPlaceSpecificator.NotOnEnd),
            new TitlePart("Risk", WordPlaceSpecificator.NotOnEnd),
            new TitlePart("Risk", WordPlaceSpecificator.NotOnEnd),
            new TitlePart("Risk", WordPlaceSpecificator.NotOnEnd),
            new TitlePart("Risk", WordPlaceSpecificator.NotOnEnd),
            new TitlePart("Risk", WordPlaceSpecificator.NotOnEnd),
            new TitlePart("KPI", WordPlaceSpecificator.NotOnEnd),
            new TitlePart("Performance", WordPlaceSpecificator.NotOnEnd),
            new TitlePart("Performance", WordPlaceSpecificator.NotOnEnd),
            new TitlePart("Performance", WordPlaceSpecificator.NotOnEnd),
            new TitlePart("Performance", WordPlaceSpecificator.NotOnEnd),
            new TitlePart("Performance", WordPlaceSpecificator.NotOnEnd),
            new TitlePart("Load", WordPlaceSpecificator.NotOnEnd),
            new TitlePart("Load", WordPlaceSpecificator.NotOnEnd),
            new TitlePart("Load", WordPlaceSpecificator.NotOnEnd),
            new TitlePart("Load", WordPlaceSpecificator.NotOnEnd),
            new TitlePart("Load", WordPlaceSpecificator.NotOnEnd),
            new TitlePart("Stress", WordPlaceSpecificator.NotOnEnd),
            new TitlePart("Stress", WordPlaceSpecificator.NotOnEnd),
            new TitlePart("Stress", WordPlaceSpecificator.NotOnEnd),
            new TitlePart("Stress", WordPlaceSpecificator.NotOnEnd),
            new TitlePart("Stress", WordPlaceSpecificator.NotOnEnd),
            new TitlePart("Experimental", WordPlaceSpecificator.NotOnEnd),
            new TitlePart("Maintenance", WordPlaceSpecificator.NotOnEnd),
            new TitlePart("Mindmap", WordPlaceSpecificator.NotOnEnd),
            new TitlePart("Unit", WordPlaceSpecificator.NotOnEnd),
            new TitlePart("Pairwise", WordPlaceSpecificator.NotOnEnd),
            new TitlePart("Visioner", WordPlaceSpecificator.OnlyOnEnd),
            new TitlePart("Process Improvement", WordPlaceSpecificator.Anywhere),
            new TitlePart("Random", WordPlaceSpecificator.Anywhere),
            new TitlePart("Reliability", WordPlaceSpecificator.Anywhere),
            new TitlePart("Robust", WordPlaceSpecificator.Anywhere),
            new TitlePart("Result-oriented", WordPlaceSpecificator.Anywhere),
            new TitlePart("Keen", WordPlaceSpecificator.Anywhere),
            new TitlePart("Infrastructure", WordPlaceSpecificator.Anywhere),
            new TitlePart("System", WordPlaceSpecificator.Anywhere),
            new TitlePart("C#", WordPlaceSpecificator.Anywhere),
            new TitlePart("Python", WordPlaceSpecificator.Anywhere),
            new TitlePart("XML", WordPlaceSpecificator.Anywhere),
            new TitlePart("HTML", WordPlaceSpecificator.Anywhere),
            new TitlePart("Erlang", WordPlaceSpecificator.Anywhere),
            new TitlePart("Elixir", WordPlaceSpecificator.Anywhere),
            new TitlePart("Java", WordPlaceSpecificator.Anywhere),
            new TitlePart("Javascript", WordPlaceSpecificator.Anywhere),
            new TitlePart("1C", WordPlaceSpecificator.Anywhere),
            new TitlePart("Haskell", WordPlaceSpecificator.Anywhere),
        };

        private static readonly Random Random = new Random();
    }

    public enum WordPlaceSpecificator
    {
        Anywhere,
        NotOnEnd,
        OnlyOnEnd
    }

    public class TitlePart
    {
        public TitlePart(string title, WordPlaceSpecificator specificator)
        {
            Title = title;
            Specificator = specificator;
        }

        public readonly string Title;
        public readonly WordPlaceSpecificator Specificator;
        public static readonly TitlePart StubTitlePart = new TitlePart("", WordPlaceSpecificator.Anywhere);

        public override bool Equals(object other)
        {
            return Title.Equals(((TitlePart) other)?.Title);
        }

        protected bool Equals(TitlePart other)
        {
            return string.Equals(Title, other.Title);
        }

        public override int GetHashCode()
        {
            return (Title != null ? Title.GetHashCode() : 0);
        }
    }
}