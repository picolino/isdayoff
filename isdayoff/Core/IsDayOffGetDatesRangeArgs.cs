using System;
using isdayoff.Contract;

namespace isdayoff.Core
{
    public class IsDayOffGetDatesRangeArgs
    {
        public DateTime From { get; }
        public DateTime To { get; }
        public Country Country { get; }
        public Region? Region { get; }
        public bool UseShortDays { get; }
        public bool TreatNonWorkingDaysByCovidAsWorkingDayAdvanced { get; }
        public bool UseSixDaysWorkWeek { get; }

        public IsDayOffGetDatesRangeArgs(
            DateTime from,
            DateTime to,
            Country country,
            Region? region,
            bool useShortDays,
            bool treatNonWorkingDaysByCovidAsWorkingDayAdvanced, 
            bool useSixDaysWorkWeek)
        {
            if (from > to)
            {
                (to, from) = (from, to);
            }
            
            From = from;
            To = to;
            Country = country;
            Region = region;
            UseShortDays = useShortDays;
            TreatNonWorkingDaysByCovidAsWorkingDayAdvanced = treatNonWorkingDaysByCovidAsWorkingDayAdvanced;
            UseSixDaysWorkWeek = useSixDaysWorkWeek;
        }
    }
}