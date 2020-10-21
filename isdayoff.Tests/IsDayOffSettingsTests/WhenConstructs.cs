using System;
using isdayoff.Contract;
using NUnit.Framework;

namespace isdayoff.Tests.IsDayOffSettingsTests
{
    internal class WhenConstructs : TestBase
    {
        [Test]
        public void IfPassedCacheIsNullArgumentNullExceptionThrows()
        {
            void Act()
            {
                _ = new IsDayOffSettings(null, Country.Russia);
            }

            Assert.Throws<ArgumentNullException>(Act);
        }

        [Test]
        public void CacheInstanceSaves()
        {
           var instance = new IsDayOffSettings(CacheStub, Country.Russia);
           
           Assert.That(instance.Cache, Is.EqualTo(CacheStub));
        }

        [Test]
        public void DefaultCountrySaves()
        {
            var instance = new IsDayOffSettings(CacheStub, Country.Russia);
           
            Assert.That(instance.DefaultCountry, Is.EqualTo(Country.Russia));
        }
    }
}