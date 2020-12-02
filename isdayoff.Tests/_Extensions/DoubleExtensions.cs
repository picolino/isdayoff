using System;

namespace isdayoff.Tests._Extensions
{
    public static class DoubleExtensions
    {
        public static DateTime Of(this double dateAsNumber, int year)
        {
            var day = (int)Math.Truncate(dateAsNumber);
            var month = (int)Math.Round(dateAsNumber * 100, 0) - (day * 100);
            
            return new DateTime(year, month, day);
        }
    }
}