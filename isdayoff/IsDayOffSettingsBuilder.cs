using System;
using isdayoff.Contract;
using isdayoff.Contract.Abstractions;
using isdayoff.Core.Cache;

namespace isdayoff
{
    /// <summary>
    /// Class for construct IsDayOff settings.
    /// Should be created only from <see cref="IsDayOffSettings.Build"/> property of <see cref="IsDayOffSettings"/> class.
    /// </summary>
    public class IsDayOffSettingsBuilder
    {
        private Country defaultCountry = Country.Russia;
        private IIsDayOffCache cache = new IsDayOffNoCache();
        
        internal IsDayOffSettingsBuilder()
        {
        }
        
        /// <summary>
        /// Set up to use built-in in-memory cache
        /// </summary>
        /// <remarks>
        /// Note that cache working per-method only. This means if you get day 
        /// off information for specific year (using <code>CheckYearAsync</code> method) 
        /// and next request trying to get information for any month (<code>CheckMonthAsync</code>) 
        /// or day (<code>CheckDayAsync</code>) of this year, additional request will be performed.
        /// However, it is likely this behavior will change in future.
        /// </remarks>
        public IsDayOffSettingsBuilder UseInMemoryCache()
        {
            cache = new IsDayOffInMemoryCache();
            return this;
        }

        /// <summary>
        /// Set up custom cache implementation
        /// </summary>
        /// <param name="customCache">Custom cache implementation</param>
        public IsDayOffSettingsBuilder UseCustomCache(IIsDayOffCache customCache)
        {
            cache = customCache;
            return this;
        }

        /// <summary>
        /// Set up default country for methods without country in parameters
        /// </summary>
        /// <param name="country">Country to set as default country</param>
        public IsDayOffSettingsBuilder UseDefaultCountry(Country country)
        {
            defaultCountry = country;
            return this;
        }

        /// <summary>
        /// Build settings
        /// </summary>
        /// <returns>Settings</returns>
        /// <exception cref="ArgumentNullException">Thrown when some not null property is set to null</exception>
        public IsDayOffSettings Create()
        {
            return new IsDayOffSettings(cache, defaultCountry);
        }

        public static implicit operator IsDayOffSettings(IsDayOffSettingsBuilder builder)
        {
            return builder.Create();
        }
    }
}