using System;
using isdayoff.Contract;

namespace isdayoff.Core.Exceptions
{
    public class BadDatesRangeException : Exception
    {
        public BadDatesRangeException(DateTime from, DateTime to, Country country) 
            : base(ErrorsMessages.DatesRangeNotSupports(from, to, country))
        {
        }
    }
}