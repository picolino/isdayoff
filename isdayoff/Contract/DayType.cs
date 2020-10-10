using JetBrains.Annotations;

namespace isdayoff.Contract
{
    [PublicAPI]
    public enum DayType
    {
        WorkingDay = 0,
        NotWorkingDay = 1,
        ShortDay = 2,
        WorkingDayAdvanced = 4
    }
}