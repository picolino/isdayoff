using System;
using isdayoff.Contract;

namespace isdayoff.Core.Exceptions
{
    public class DayOffDataNotFoundException : Exception
    {
        public DayOffDataNotFoundException(DateTime from, DateTime to, Country country) 
            : base(ErrorsMessages.CanNotFindDayOffInfo(from, to, country))
        {
        }
    }
}