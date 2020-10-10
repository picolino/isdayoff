using System;
using isdayoff.Contract;

namespace isdayoff.Core.Exceptions
{
    public class DayOffDataNotFoundException : Exception
    {
        public DayOffDataNotFoundException(int year, int? month, int? day, Country country) 
            : base( ErrorsMessages.CanNotFindDayOffInfo(year, month, day, country))
        {
        }
    }
}