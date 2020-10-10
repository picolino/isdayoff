using System;
using isdayoff.Contract;
using isdayoff.Contract.Abstractions;
using JetBrains.Annotations;

namespace isdayoff
{
    [PublicAPI]
    public class IsDayOffSettings
    {
        /// <summary>
        /// Settings builder for settings creation
        /// </summary>
        [NotNull]
        public static IsDayOffSettingsBuilder Build => new IsDayOffSettingsBuilder();
        
        /// <summary>
        /// Default settings
        /// </summary>
        [NotNull]
        public static IsDayOffSettings Default => Build.Create();
        
        internal IsDayOffSettings(IIsDayOffCache cache, Country defaultCountry)
        {
            Cache = cache ?? throw new ArgumentNullException(nameof(cache), 
                "Cache implementation can't be null. Cache is disabled by-default so you dont need to set it to null.");
            DefaultCountry = defaultCountry;
        }
        
        [NotNull]
        internal IIsDayOffCache Cache { get; }
        internal Country DefaultCountry { get; }
    }
}