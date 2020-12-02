using System;

namespace isdayoff.Core.Exceptions
{
    public class DaysCountMismatchException : Exception
    {
        public DaysCountMismatchException(int requestDaysCount, int responseDaysCount)
            : base(ErrorsMessages.DaysCountMismatch(requestDaysCount, responseDaysCount))
        {
        }
    }
}