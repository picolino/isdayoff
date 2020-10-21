using isdayoff.Contract;
using isdayoff.Core.Cache;
using NUnit.Framework;

namespace isdayoff.Tests.IsDayOffSettingsTests
{
    internal class WhenDefault : IsDayOffSettingsTestBase
    {
        [Test]
        public void DefaultAlwaysReturnsUniqueSettings()
        {
            var defaultSettingsFirst = IsDayOffSettings.Default;
            var defaultSettingsSecond = IsDayOffSettings.Default;

            Assert.That(defaultSettingsFirst, Is.Not.EqualTo(defaultSettingsSecond));
        }

        [Test]
        public void DefaultCountryIsRussia()
        {
            var defaultSettings = IsDayOffSettings.Default;

            Assert.That(defaultSettings.DefaultCountry, Is.EqualTo(Country.Russia));
        }

        [Test]
        public void DefaultCacheIsNoCache()
        {
            var defaultSettings = IsDayOffSettings.Default;

            Assert.That(defaultSettings.Cache, Is.TypeOf<IsDayOffNoCache>());
        }
    }
}