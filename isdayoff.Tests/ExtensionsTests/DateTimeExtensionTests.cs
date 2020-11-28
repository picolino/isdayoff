using System.Linq;
using isdayoff.Core.Extensions;
using isdayoff.Tests.Extensions;
using NUnit.Framework;

namespace isdayoff.Tests.ExtensionsTests
{
    internal class DateTimeExtensionTests : TestBase
    {
        [Test]
        public void StartOfMonthWorkingCorrectly()
        {
            var dt = 15.01.Of(2020);

            var startOfMonth = dt.StartOfMonth();
            
            Assert.That(startOfMonth, Is.EqualTo(01.01.Of(2020)));
        }
        
        [Test]
        public void StartOfYearWorkingCorrectly()
        {
            var dt = 15.04.Of(2020);

            var startOfMonth = dt.StartOfYear();
            
            Assert.That(startOfMonth, Is.EqualTo(01.01.Of(2020)));
        }

        [Test]
        public void EndOfMonthWorkingCorrectly()
        {
            var dt = 15.01.Of(2020);

            var startOfMonth = dt.EndOfMonth();
            
            Assert.That(startOfMonth, Is.EqualTo(01.02.Of(2020).AddMilliseconds(-1)));
        }
        
        [Test]
        public void EndOfYearWorkingCorrectly()
        {
            var dt = 15.01.Of(2020);

            var startOfMonth = dt.EndOfYear();
            
            Assert.That(startOfMonth, Is.EqualTo(31.12.Of(2020).EndOfMonth()));
        }

        [Test]
        public void ByDaysTillWorkingCorrectly()
        {
            var dtStart = 01.01.Of(2020);
            var dtEnd = 07.01.Of(2020);

            var days = dtStart.ByDaysTill(dtEnd);

            Assert.That(days.Count(), Is.EqualTo(7));
        }
    }
}