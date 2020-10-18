using System;

namespace isdayoff.Tests.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime At(this DateTime dateTime, double time)
        {
            var hour = (int) Math.Truncate(time);
            var minute = (int) Math.Round(time * 100, 0) - (hour * 100);
            
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, hour, minute, 0);
        }
    }
}