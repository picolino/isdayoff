using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using isdayoff.Contract;
using isdayoff.Core;

namespace isdayoff
{
    //TODO: DO NOT SUPPORT ISYEARLEAF CUZ WE ALREADY HAS BUILT-IN FUNCTION IN DOTNET
    public class IsDayOff
    {
        private readonly IsDayOffSettings settings;
        private readonly IsDayOffService service;
        
        public IsDayOff() : this(IsDayOffSettings.Default)
        {
        }

        public IsDayOff(IsDayOffSettings settings)
        {
            this.settings = settings;
            service = new IsDayOffService(settings, new IsDayOffApiClient("https://isdayoff.ru/api/"));
        }

        public async Task<List<DayOffDateTime>> CheckYearAsync(int year, DayOffCountry country = DayOffCountry.Russia)
        {
            return await service.CheckYearAsync(year, country);
        }

        public async Task<List<DayOffDateTime>> CheckMonthAsync(int year, int month, DayOffCountry country = DayOffCountry.Russia)
        {
            return await service.CheckMonthAsync(year, month, country);
        }
        
        public async Task<DayType> CheckDayAsync(int year, int month, int day, DayOffCountry country = DayOffCountry.Russia)
        {
            return await service.CheckDayAsync(year, month, day, country);
        }
        
        public async Task<DayType> CheckDayAsync(DateTime day, DayOffCountry country = DayOffCountry.Russia)
        {
            return await CheckDayAsync(day.Year, day.Month, day.Day, country);
        }
    }
}