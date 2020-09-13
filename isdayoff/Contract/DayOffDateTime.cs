using System;

namespace isdayoff.Contract
{
    public readonly struct DayOffDateTime
    {
        public DayOffDateTime(DateTime dateTime, DayType dayType)
        {
            DateTime = dateTime;
            DayType = dayType;
        }

        public DateTime DateTime { get; }
        public DayType DayType { get; }

        public override string ToString()
        {
            return $"{DateTime} ({DayType})";
        }
    }
}