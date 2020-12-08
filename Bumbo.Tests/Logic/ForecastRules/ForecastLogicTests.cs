using System.Collections.Generic;
using Bumbo.Data.Models;
using Bumbo.Data.Models.Common;
using Bumbo.Data.Models.Enums;
using Bumbo.Logic.Forecast;
using NUnit.Framework;

namespace Bumbo.Tests.Logic.ForecastRules
{
    public class ForecastLogicTests
    {
        private readonly ForecastLogic _logic;
        public ForecastLogicTests()
        {
            _logic = new ForecastLogic(new List<IForecastStandard>()
            {
                new ForecastStandard()
                {
                    Value = 10,
                    Activity = ForecastActivity.STOCK_SHELVES
                },
                new ForecastStandard()
                {
                    Value = 10,
                    Activity = ForecastActivity.CASHIER
                },
                new ForecastStandard()
                {
                    Value = 10,
                    Activity = ForecastActivity.FACE_SHELVES
                },
                new ForecastStandard()
                {
                    Value = 10,
                    Activity = ForecastActivity.PRODUCE_DEPARTMENT
                },
                new ForecastStandard()
                {
                    Value = 10,
                    Activity = ForecastActivity.UNLOAD_COLI
                }
            });
        }

        [Test]
        public void WorkHoursFacing_ShouldReturn0()
        {
            Assert.Zero(_logic.WorkHoursFacingShelves(0));
        }

        [Test]
        public void WorkHoursStocking_ShouldReturn0()
        {
            Assert.Zero(_logic.WorkHoursStockingColi(0));
        }


        [Test]
        public void WorkHoursUnloading_ShouldReturn0()
        {
            Assert.Zero(_logic.WorkHoursUnloading(0));
        }

        [Test]
        public void WorkHoursStockClerk_ShouldReturn0()
        {
            Assert.Zero(_logic.GetWorkHoursStockClerk(0, 0));
        }

        [Test]
        public void LessColiShould_ReturnLessWorkHours()
        {
            for (var i = 0; i < 50; i++)
            {
                var manyColi = _logic.GetWorkHoursStockClerk(0, i + 1);
                var fewColi = _logic.GetWorkHoursStockClerk(0, i);

                Assert.Greater(manyColi, fewColi);
            }
        }

        [Test]
        public void FewerMetersShould_ReturnLessWorkHours()
        {
            for (var i = 0; i < 50; i++)
            {
                var manyColi = _logic.GetWorkHoursStockClerk(i + 1, 0);
                var fewColi = _logic.GetWorkHoursStockClerk(i, 0);

                Assert.Greater(manyColi, fewColi);
            }
        }

        [Test]
        public void FewerMeters_AndFewColiShould_ReturnLessWorkHours()
        {
            for (var i = 0; i < 50; i++)
            {
                var manyColi = _logic.GetWorkHoursStockClerk(i + 1, i + 1);
                var fewColi = _logic.GetWorkHoursStockClerk(i, i);

                Assert.Greater(manyColi, fewColi);
            }
        }

        [Test]
        public void WorkHoursFacing_ShouldReturn()
        {
            Assert.AreEqual((decimal)0.83, _logic.WorkHoursFacingShelves(5));
        }
    }
}
