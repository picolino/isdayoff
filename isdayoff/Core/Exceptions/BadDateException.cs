using System;
using isdayoff.Contract;

namespace isdayoff.Core.Exceptions
{
    public class BadDateException : Exception
    {
        public BadDateException(int year, int? month, int? day, Country country) 
            : base($"Date {year}{(month is null ? "" : $"-{month}")}{(day is null ? "" : $"-{day}")} for country {country} not supports")
        {
        }
    }
}