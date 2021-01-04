using System;
using Bumbo.Data.Models;
using Bumbo.Logic.PayCheck;
using NUnit.Framework;

namespace Bumbo.Tests.Logic.PayCheckRules
{
    public class PayCheckLogicTests
    {
        private readonly PayCheckLogic _logic;

        public PayCheckLogicTests()
        {
            _logic = new PayCheckLogic();
        }

        [Test]
        public void BonusTimeBetween00And06ShouldReturn0WhenNotWorkedInTimeSpan()
        {
            var data = new WorkedShift();
            data.StartTime = new TimeSpan(12, 0, 0);
            data.EndTime = new TimeSpan(14, 0, 0);

            var result = _logic.BonusTimeBetween00And06(data);

            Assert.Zero(result.TotalMinutes);
        }

        [Test]
        public void BonusTimeBetween18And24ShouldReturn0WhenNotWorkedInTimeSpan()
        {
            var data = new WorkedShift();
            data.StartTime = new TimeSpan(12, 0, 0);
            data.EndTime = new TimeSpan(14, 0, 0);

            var result = _logic.BonusTimeBetween18And24(data);

            Assert.Zero(result.TotalMinutes);
        }

        [Test]
        public void BonusTimeBetween20And21ShouldReturn0WhenNotWorkedInTimeSpan()
        {
            var data = new WorkedShift();
            data.StartTime = new TimeSpan(12, 0, 0);
            data.EndTime = new TimeSpan(14, 0, 0);

            var result = _logic.BonusTimeBetween20And21(data);

            Assert.Zero(result.TotalMinutes);
        }

        [Test]
        public void BonusTimeBetween21And24ShouldReturn0WhenNotWorkedInTimeSpan()
        {
            var data = new WorkedShift();
            data.StartTime = new TimeSpan(12, 0, 0);
            data.EndTime = new TimeSpan(14, 0, 0);

            var result = _logic.BonusTimeBetween21And24(data);

            Assert.Zero(result.TotalMinutes);
        }

        [Test]
        public void BonusTimeBetween00And06ShouldReturn2WhenWorked2HoursInTimeSpan()
        {
            var data = new WorkedShift();
            data.StartTime = new TimeSpan(4, 0, 0);
            data.EndTime = new TimeSpan(8, 0, 0);

            var result = _logic.BonusTimeBetween00And06(data);

            Assert.AreEqual(result.TotalMinutes, 120);
        }

        [Test]
        public void BonusTimeBetween18And24ShouldReturn2WhenWorked2HoursInTimeSpan()
        {
            var data = new WorkedShift();
            data.StartTime = new TimeSpan(16, 0, 0);
            data.EndTime = new TimeSpan(20, 0, 0);

            var result = _logic.BonusTimeBetween18And24(data);

            Assert.AreEqual(result.TotalMinutes, 120);
        }

        [Test]
        public void BonusTimeBetween20And21ShouldReturn1WhenWorked1HoursInTimeSpan()
        {
            var data = new WorkedShift();
            data.StartTime = new TimeSpan(19, 0, 0);
            data.EndTime = new TimeSpan(22, 0, 0);

            var result = _logic.BonusTimeBetween20And21(data);

            Assert.AreEqual(result.TotalMinutes, 60);
        }

        [Test]
        public void BonusTimeBetween21And24ShouldReturn2WhenWorked2HoursInTimeSpan()
        {
            var data = new WorkedShift();
            data.StartTime = new TimeSpan(20, 0, 0);
            data.EndTime = new TimeSpan(23, 0, 0);

            var result = _logic.BonusTimeBetween21And24(data);

            Assert.AreEqual(result.TotalMinutes, 120);
        }

        [Test]
        public void RegularShiftShouldHaveCorrectPaycheck()
        {
            var data = new WorkedShift();
            data.Sick = false;
            // deze datum is een maandag
            data.Shift = new Shift { Date = new DateTime(2020, 12, 7) };
            data.StartTime = new TimeSpan(5, 0, 0);
            data.EndTime = new TimeSpan(22, 0, 0);

            var checkResult = new PayCheck();
            checkResult.AddTime(1.5, new TimeSpan(1, 0, 0));
            checkResult.AddTime(1.33, new TimeSpan(1, 0, 0));
            checkResult.AddTime(1.5, new TimeSpan(1, 0, 0));
            checkResult.AddTime(1, new TimeSpan(12, 0, 0));

            var result = _logic.CalculateBonus(data);

            Assert.AreEqual(result.GetTime(1.5).TotalMinutes, checkResult.GetTime(1.5).TotalMinutes);
            Assert.AreEqual(result.GetTime(1.33).TotalMinutes, checkResult.GetTime(1.33).TotalMinutes);
            Assert.AreEqual(result.GetTime(1).TotalMinutes, checkResult.GetTime(1).TotalMinutes);
        }

        [Test]
        public void ShiftsOnSundayShouldHave200PayRate()
        {
            var data = new WorkedShift();
            data.Sick = false;
            // deze datum is een zondag
            data.Shift = new Shift { Date = new DateTime(2020, 12, 6) };
            data.StartTime = new TimeSpan(12, 0, 0);
            data.EndTime = new TimeSpan(18, 0, 0);

            var checkResult = new PayCheck();
            checkResult.AddTime(2.0, new TimeSpan(6, 0, 0));

            var result = _logic.CalculateBonus(data);

            Assert.AreEqual(result.GetTime(2.0).TotalMinutes, checkResult.GetTime(2.0).TotalMinutes);
        }

        [Test]
        public void SickShiftsShouldHave70PayRate()
        {
            var data = new WorkedShift();
            data.Sick = true;
            // deze datum is een zondag
            data.Shift = new Shift { Date = new DateTime(2020, 12, 6) };
            data.StartTime = new TimeSpan(12, 0, 0);
            data.EndTime = new TimeSpan(18, 0, 0);

            var checkResult = new PayCheck();
            checkResult.AddTime(0.7, new TimeSpan(6, 0, 0));

            var result = _logic.CalculateBonus(data);

            Assert.AreEqual(result.GetTime(0.7).TotalMinutes, checkResult.GetTime(0.7).TotalMinutes);
        }


    }
}
