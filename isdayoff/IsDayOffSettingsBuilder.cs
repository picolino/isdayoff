using System;
using System.Diagnostics;
using System.Reflection;
using isdayoff.Contract;
using isdayoff.Contract.Abstractions;
using isdayoff.Core.Cache;
using isdayoff.Core.Exceptions;
using JetBrains.Annotations;

namespace isdayoff
{
    /// <summary>
    /// Class for construct IsDayOff settings.
    /// Should be created only from <see cref="IsDayOffSettings.Build"/> property of <see cref="IsDayOffSettings"/> class.
    /// </summary>
    [PublicAPI]
    public class IsDayOffSettingsBuilder
    {
        private const string ApiBaseUrl = "https://isdayoff.ru/api/";
        
        private Country defaultCountry = Country.Russia;
        private IIsDayOffCache cache = new IsDayOffNoCache();
        private SourceLevels? logLevel = null;

        private readonly string userAgent;
        
        internal IsDayOffSettingsBuilder()
        {
            var currentAssemblyVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString(2);
            userAgent = $"isdayoff-dotnet-lib/{currentAssemblyVersion} (lib-maintainer:picolino;lib-feedback:contact@picolino.dev)";
        }
        
        /// <summary>
        /// Set up to use built-in in-memory cache
        /// </summary>
        [NotNull]
        public IsDayOffSettingsBuilder UseInMemoryCache()
        {
            cache = new IsDayOffInMemoryCache();
            return this;
        }

        /// <summary>
        /// Set up custom cache implementation
        /// </summary>
        /// <param name="newCache">Custom cache implementation</param>
        /// <exception cref="ArgumentNullException">Thrown when customCache is set to null</exception>
        [NotNull]
        public IsDayOffSettingsBuilder UseCustomCache([NotNull] IIsDayOffCache newCache)
        {
            cache = newCache ?? throw new ArgumentNullException(nameof(cache), ErrorsMessages.CacheCanNotBeNull());
            return this;
        }

        /// <summary>
        /// Set up default country for methods without country in parameters
        /// </summary>
        /// <param name="newDefaultCountry">Country to set as default country</param>
        [NotNull]
        public IsDayOffSettingsBuilder UseDefaultCountry(Country newDefaultCountry)
        {
            defaultCountry = newDefaultCountry;
            return this;
        }

        /// <summary>
        /// Enable logging with log level specified
        /// </summary>
        /// <param name="newLogLevel">Tracing log level</param>
        [NotNull]
        public IsDayOffSettingsBuilder UseLogging(SourceLevels newLogLevel)
        {
            logLevel = newLogLevel;
            return this;
        }

        /// <summary>
        /// Build settings
        /// </summary>
        /// <returns>Settings</returns>
        [NotNull]
        public IsDayOffSettings Create()
        {
            return new IsDayOffSettings(ApiBaseUrl, userAgent, cache, defaultCountry, logLevel);
        }

        [NotNull]
        public static implicit operator IsDayOffSettings(IsDayOffSettingsBuilder builder)
        {
            return builder.Create();
        }
    }
}