using System;
using Bumbo.Logic.Forecast;
using NUnit.Framework;

namespace Bumbo.Tests.Logic.DateLogicRules
{
    public class DateLogicTests
    {
        [Test]
        public void InvalidWeekInYearShouldFail()
        {
            Assert.Throws<ArgumentException>(() => DateLogic.DateFromWeekNumber(2019, 555));
        }

        [Test]
        public void ZeroWeekShouldFail()
        {
            Assert.Throws<ArgumentException>(() => DateLogic.DateFromWeekNumber(2019, 0));
        }

        [Test]
        public void NegativeWeekShouldFail()
        {
            Assert.Throws<ArgumentException>(() => DateLogic.DateFromWeekNumber(2019, -1));
        }

        [Test]
        public void ValidWeekInYearShould_ReturnCorrectDate()
        {
            // Dates can be checked here: http://myweb.ecu.edu/mccartyr/isowdcal.html
            var expectedDate = new DateTime(2021, 1, 4);
            
            Assert.AreEqual(expectedDate, DateLogic.DateFromWeekNumber(2021, 1));
        }


    }
}