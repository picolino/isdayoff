using isdayoff.Contract;
using isdayoff.Core.Cache;
using NUnit.Framework;

namespace isdayoff.Tests.IsDayOffSettingsTests
{
    internal class WhenBuild : IsDayOffSettingsTestBase
    {
        [Test]
        public void BuildAlwaysReturnsUniqueSettingsBuilder()
        {
            var builderFirst = IsDayOffSettings.Build;
            var builderSecond = IsDayOffSettings.Build;

            Assert.That(builderFirst, Is.Not.EqualTo(builderSecond));
        }

        [Test]
        public void DefaultCountryIsEqualToDefaultCountryFromBuilder([Values] Country country)
        {
            var builder = IsDayOffSettings.Build.UseDefaultCountry(country).Create();

            Assert.That(builder.DefaultCountry, Is.EqualTo(country));
        }

        [Test]
        public void InMemoryCacheEnabled()
        {
            var builder = IsDayOffSettings.Build.UseInMemoryCache().Create();

            Assert.That(builder.Cache, Is.TypeOf<IsDayOffInMemoryCache>());
        }
    }
}