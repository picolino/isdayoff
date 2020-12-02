using System;
using System.Diagnostics;
using isdayoff.Contract;
using isdayoff.Contract.Abstractions;
using isdayoff.Core.Exceptions;
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
        
        [NotNull]
        internal IIsDayOffCache Cache { get; }
        internal Country DefaultCountry { get; }
        internal string ApiBaseUrl { get; } 
        internal string UserAgent { get; } 
        internal SourceLevels? TraceLevel { get; }
    }
}