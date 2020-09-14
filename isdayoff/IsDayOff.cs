using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using isdayoff.Contract;
using isdayoff.Core;
using isdayoff.Core.Exceptions;

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

        /// <summary>
        /// Get dates with day off information for year
        /// </summary>
        /// <param name="year">Year to get dates with day off information</param>
        /// <param name="country">Country to get dates with day off information for</param>
        /// <returns>List of dates with day off information</returns>
        /// <exception cref="IsDayOffExternalServiceException">Throws if error occured while processing request to isdayoff external service</exception>
        public async Task<List<DayOffDateTime>> CheckYearAsync(int year, DayOffCountry country = DayOffCountry.Russia)
        {
            return await service.CheckYearAsync(year, country);
        }

        /// <summary>
        /// Get dates with day off information for month of specific year
        /// </summary>
        /// <param name="year">Year of month</param>
        /// <param name="month">Month to get dates with day off information</param>
        /// <param name="country">Country to get dates with day off information for</param>
        /// <returns>List of dates with day off information</returns>
        /// <exception cref="IsDayOffExternalServiceException">Throws if error occured while processing request to isdayoff external service</exception>
        public async Task<List<DayOffDateTime>> CheckMonthAsync(int year, int month, DayOffCountry country = DayOffCountry.Russia)
        {
            return await service.CheckMonthAsync(year, month, country);
        }

        /// <summary>
        /// Get day off information for day of specific month of specific year
        /// </summary>
        /// <param name="year">Year of day</param>
        /// <param name="month">Month of day</param>
        /// <param name="day">Day to get day off information</param>
        /// <param name="country">Country to get day off information for</param>
        /// <returns>Day off information</returns>
        /// <exception cref="IsDayOffExternalServiceException">Throws if error occured while processing request to isdayoff external service</exception>
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