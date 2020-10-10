using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using isdayoff.Contract;
using isdayoff.Core;
using isdayoff.Core.Exceptions;
using JetBrains.Annotations;

namespace isdayoff
{
    /// <summary>
    /// Basic class for operating with isdayoff API
    /// </summary>
    [PublicAPI]
    public class IsDayOff
    {
        private readonly IsDayOffSettings settings;
        private readonly IsDayOffService service;
        
        private const string ApiBaseUrl = "https://isdayoff.ru/api/";
        
        /// <summary>
        /// Makes IsDayOff with default settings.
        /// Default country sets to Russia and no in-memory cache use.
        /// </summary>
        public IsDayOff() : this(IsDayOffSettings.Default)
        {
        }

        /// <summary>
        /// Makes IsDayOff with overriden settings.
        /// </summary>
        /// <param name="settings">Settings</param>
        public IsDayOff(IsDayOffSettings settings)
        {
            this.settings = settings ?? throw new ArgumentNullException(nameof(settings));
            service = new IsDayOffService(new IsDayOffApiClient(ApiBaseUrl), settings.Cache);
        }

        /// <summary>
        /// Get dates with day off information for year
        /// </summary>
        /// <param name="year">Year to get dates with day off information</param>
        /// <param name="country">Country to get dates with day off information for</param>
        /// <returns>List of dates with day off information</returns>
        /// <exception cref="IsDayOffExternalServiceException">Throws if error occured while processing request to isdayoff external service</exception>
        public async Task<List<DayOffDateTime>> CheckYearAsync(int year, Country country)
        {
            return await service.CheckYearAsync(year, country);
        }
        
        /// <summary>
        /// Get dates with day off information for year of default country
        /// </summary>
        /// <param name="year">Year to get dates with day off information</param>
        /// <returns>List of dates with day off information</returns>
        /// <exception cref="IsDayOffExternalServiceException">Throws if error occured while processing request to isdayoff external service</exception>
        public async Task<List<DayOffDateTime>> CheckYearAsync(int year)
        {
            return await CheckYearAsync(year, settings.DefaultCountry);
        }

        /// <summary>
        /// Get dates with day off information for month of specific year
        /// </summary>
        /// <param name="year">Year of month</param>
        /// <param name="month">Month to get dates with day off information</param>
        /// <param name="country">Country to get dates with day off information for</param>
        /// <returns>List of dates with day off information</returns>
        /// <exception cref="IsDayOffExternalServiceException">Throws if error occured while processing request to isdayoff external service</exception>
        public async Task<List<DayOffDateTime>> CheckMonthAsync(int year, int month, Country country)
        {
            return await service.CheckMonthAsync(year, month, country);
        }
        
        /// <summary>
        /// Get dates with day off information for month of specific year of default country
        /// </summary>
        /// <param name="year">Year of month</param>
        /// <param name="month">Month to get dates with day off information</param>
        /// <returns>List of dates with day off information</returns>
        /// <exception cref="IsDayOffExternalServiceException">Throws if error occured while processing request to isdayoff external service</exception>
        public async Task<List<DayOffDateTime>> CheckMonthAsync(int year, int month)
        {
            return await CheckMonthAsync(year, month, settings.DefaultCountry);
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
        public async Task<DayType> CheckDayAsync(int year, int month, int day, Country country)
        {
            return await service.CheckDayAsync(year, month, day, country);
        }
        
        /// <summary>
        /// Get day off information for day of specific month of specific year of default country
        /// </summary>
        /// <param name="year">Year of day</param>
        /// <param name="month">Month of day</param>
        /// <param name="day">Day to get day off information</param>
        /// <returns>Day off information</returns>
        /// <exception cref="IsDayOffExternalServiceException">Throws if error occured while processing request to isdayoff external service</exception>
        public async Task<DayType> CheckDayAsync(int year, int month, int day)
        {
            return await CheckDayAsync(year, month, day, settings.DefaultCountry);
        }
        
        /// <summary>
        /// Get day off information for day of specific month of specific year
        /// </summary>
        /// <param name="day">DateTime of specific day to get day off information</param>
        /// <param name="country">Country to get day off information for</param>
        /// <returns>Day off information</returns>
        /// <exception cref="IsDayOffExternalServiceException">Throws if error occured while processing request to isdayoff external service</exception>
        public async Task<DayType> CheckDayAsync(DateTime day, Country country)
        {
            return await CheckDayAsync(day.Year, day.Month, day.Day, country);
        }
        
        /// <summary>
        /// Get day off information for day of specific month of specific year of default country
        /// </summary>
        /// <param name="day">DateTime of specific day to get day off information</param>
        /// <returns>Day off information</returns>
        /// <exception cref="IsDayOffExternalServiceException">Throws if error occured while processing request to isdayoff external service</exception>
        public async Task<DayType> CheckDayAsync(DateTime day)
        {
            return await CheckDayAsync(day, settings.DefaultCountry);
        }
    }
}