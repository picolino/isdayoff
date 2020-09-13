using isdayoff.Contract;

namespace isdayoff
{
    public class IsDayOffSettings
    {
        public static IsDayOffSettings Default => new IsDayOffSettings();
        
        public bool UseCache { get; }
    }
}