using System.Diagnostics;
using isdayoff.Contract;
using NUnit.Framework;

namespace isdayoff.Tests.IsDayOffSettingsTests
{
    internal class WhenConstructs : TestBase
    {
        [Test]
        public void CacheInstanceSaves()
        {
            var instance = new IsDayOffSettings(
                ApiBaseUrlStub,
                UserAgentStub,
                CacheStub,
                Country.Russia,
                false,
                false,
                false,
                SourceLevels.Off);
           
           Assert.That(instance.Cache, Is.EqualTo(CacheStub));
        }

        [Test]
        public void DefaultCountrySaves()
        {
            var instance = new IsDayOffSettings(
                ApiBaseUrlStub,
                UserAgentStub,
                CacheStub,
                Country.Russia,
                false,
                false,
                false,
                SourceLevels.Off);
           
            Assert.That(instance.DefaultCountry, Is.EqualTo(Country.Russia));
        }

        [Test]
        public void ApiBaseUrlSaves()
        {
            var instance = new IsDayOffSettings(
                ApiBaseUrlStub,
                UserAgentStub,
                CacheStub,
                Country.Russia,
                false,
                false,
                false,
                SourceLevels.Off);
           
            Assert.That(instance.ApiBaseUrl, Is.EqualTo(ApiBaseUrlStub));
        }

        [Test]
        public void UserAgentSaves()
        {
            var instance = new IsDayOffSettings(
                ApiBaseUrlStub,
                UserAgentStub,
                CacheStub,
                Country.Russia,
                false,
                false,
                false,
                SourceLevels.Off);
           
            Assert.That(instance.UserAgent, Is.EqualTo(UserAgentStub));
        }

        [Test]
        public void TraceLevelSaves()
        {
            var instance = new IsDayOffSettings(
                ApiBaseUrlStub,
                UserAgentStub,
                CacheStub,
                Country.Russia,
                false,
                false,
                false,
                SourceLevels.Information);
           
            Assert.That(instance.TraceLevel, Is.EqualTo(SourceLevels.Information));
        }
    }
}