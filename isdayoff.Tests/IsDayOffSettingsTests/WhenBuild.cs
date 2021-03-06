﻿using System;
using System.Diagnostics;
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
        public void TraceLevelIsEqualToPassedTraceLevel([Values] SourceLevels traceLevel)
        {
            var builder = IsDayOffSettings.Build.UseLogging(traceLevel).Create();

            Assert.That(builder.TraceLevel, Is.EqualTo(traceLevel));
        }

        [Test]
        public void UseInMemoryCacheInMemoryCacheEnabled()
        {
            var builder = IsDayOffSettings.Build.UseInMemoryCache().Create();

            Assert.That(builder.Cache, Is.TypeOf<IsDayOffInMemoryCache>());
        }

        [Test]
        public void CustomCacheCanNotBeNull()
        {
            void Act()
            {
                IsDayOffSettings.Build.UseCustomCache(null).Create();
            }

            Assert.Throws<ArgumentNullException>(Act);
        }

        [Test]
        public void CustomCacheIsEqualToPassedCache()
        {
            var cache = new IsDayOffNoCache();
            
            var builder = IsDayOffSettings.Build.UseCustomCache(cache).Create();

            Assert.That(builder.Cache, Is.EqualTo(cache));
        }

        [Test]
        public void IsDayOffSettingsBuilderCanImplicitIsDayOffSettings()
        {
            IsDayOffSettings _ = IsDayOffSettings.Build;

            Assert.Pass();
        }
    }
}