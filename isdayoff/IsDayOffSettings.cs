using System;
using isdayoff.Contract;
using isdayoff.Contract.Abstractions;

namespace isdayoff
{
    public class IsDayOffSettings
    {
        /// <summary>
        /// Settings builder for settings creation
        /// </summary>
        public static IsDayOffSettingsBuilder Build => new IsDayOffSettingsBuilder();
        
        /// <summary>
        /// Default settings
        /// </summary>
        public static IsDayOffSettings Default => Build.Create();
        
        internal IsDayOffSettings(IIsDayOffCache cache, Country defaultCountry)
        {
            Cache = cache ?? throw new ArgumentNullException(nameof(cache), 
                "Cache implementation can't be null. Cache is disabled by-default so you dont need to set it to null.");
            DefaultCountry = defaultCountry;
        }
        
        internal IIsDayOffCache Cache { get; }
        internal Country DefaultCountry { get; }
    }
}