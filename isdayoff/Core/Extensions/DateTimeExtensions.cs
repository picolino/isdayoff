using System;
using System.Collections.Generic;

namespace isdayoff.Core.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime StartOfYear(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, 1, 1);
        }
        
        public static DateTime EndOfYear(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, 12, 1).EndOfMonth();
        }
        
        public static DateTime StartOfMonth(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, 1);
        }
        
        public static DateTime EndOfMonth(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, 1).AddMonths(1).AddMilliseconds(-1);
        }

        public static IEnumerable<DateTime> ByDaysTill(this DateTime dateTime, DateTime end)
        {
            for (var date = dateTime; date <= end; date = date.AddDays(1))
            {
                yield return date;
            }
        }
        
    }
}