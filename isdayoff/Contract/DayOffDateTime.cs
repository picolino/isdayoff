using System;
using System.Diagnostics;

namespace isdayoff.Contract
{
    /// <summary>
    /// Provides day off information about day and day itself 
    /// </summary>
    public readonly struct DayOffDateTime
    {
        /// <summary>
        /// Create new day with dayoff information
        /// </summary>
        /// <param name="dateTime">Day</param>
        /// <param name="dayType">Day off information</param>
        public DayOffDateTime(DateTime dateTime, DayType dayType)
        {
            DateTime = dateTime;
            DayType = dayType;
        }

        /// <summary>
        /// Day
        /// </summary>
        public DateTime DateTime { get; }
        
        /// <summary>
        /// Day off information
        /// </summary>
        public DayType DayType { get; }

        public override string ToString()
        {
            return $"{DateTime} ({DayType})";
        }
    }
}