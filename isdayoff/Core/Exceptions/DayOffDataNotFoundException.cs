using System;
using isdayoff.Contract;

namespace isdayoff.Core.Exceptions
{
    public class DayOffDataNotFoundException : Exception
    {
        public DayOffDataNotFoundException(int year, int? month, int? day, Country country) 
            : base($"Cannot find day off information on date {year}{(month is null ? "" : $"-{month}")}{(day is null ? "" : $"-{day}")} for country {country}")
        {
        }
    }
}