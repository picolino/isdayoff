using System;
using isdayoff.Contract;

namespace isdayoff.Core.Exceptions
{
    public class BadDateException : Exception
    {
        public BadDateException(int year, int? month, int? day, Country country) 
            : base(ErrorsMessages.DateNotSupports(year, month, day, country))
        {
        }
    }
}