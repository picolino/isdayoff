using isdayoff.Contract;
using isdayoff.Contract.Abstractions;

namespace isdayoff
{
    public class IsDayOffSettings
    {
        public IsDayOffSettings(IIsDayOffCache cache, Country defaultCountry)
        {
            Cache = cache;
            DefaultCountry = defaultCountry;
        }
        
        public IIsDayOffCache Cache { get; }
        public Country DefaultCountry { get; }
    }
}