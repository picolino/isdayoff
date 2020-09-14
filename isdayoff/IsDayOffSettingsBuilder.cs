using isdayoff.Contract;
using isdayoff.Contract.Abstractions;
using isdayoff.Core.Cache;

namespace isdayoff
{
    public class IsDayOffSettingsBuilder
    {
        private Country defaultCountry = Country.Russia;
        private IIsDayOffCache cache = new IsDayOffNoCache();
        
        public IsDayOffSettingsBuilder UseInMemoryCache()
        {
            cache = new IsDayOffInMemoryCache();
            return this;
        }

        public IsDayOffSettingsBuilder UseCustomCache(IIsDayOffCache customCache)
        {
            cache = customCache;
            return this;
        }

        public IsDayOffSettingsBuilder UseDefaultCountry(Country country)
        {
            defaultCountry = country;
            return this;
        }

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