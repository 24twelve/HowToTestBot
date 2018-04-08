using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace EuristicsBot
{
    internal class Tests
    {
        [Test]
        public void JobTitleGetter_GeneratesSomething()
        {
            var randomJobTitle = JobTitleGetter.GetRandomJobTitle();
            randomJobTitle.Should().NotBe("");
        }

        [Test]
        [Explicit]
        public void GenerateManyToShow()
        {
            for (var i = 0; i < 1000; i++)
                Console.WriteLine(JobTitleGetter.GetRandomJobTitle());
        }
    }
}