using System.Diagnostics;
using isdayoff.Contract;
using isdayoff.Contract.Abstractions;

namespace isdayoff
{
    /// <summary>
    /// Settings for IsDayOff
    /// </summary>
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
        
        internal IsDayOffSettings(string apiBaseUrl, 
                                  string userAgent, 
                                  IIsDayOffCache cache, 
                                  Country defaultCountry, 
                                  SourceLevels? traceLevel)
        {
            ApiBaseUrl = apiBaseUrl;
            UserAgent = userAgent;
            Cache = cache;
            DefaultCountry = defaultCountry;
            TraceLevel = traceLevel;
        }
        
        internal IIsDayOffCache Cache { get; }
        internal Country DefaultCountry { get; }
        internal string ApiBaseUrl { get; } 
        internal string UserAgent { get; } 
        internal SourceLevels? TraceLevel { get; }
    }
}