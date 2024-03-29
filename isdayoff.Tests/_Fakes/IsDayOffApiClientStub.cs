﻿using System;
using System.Threading;
using System.Threading.Tasks;
using isdayoff.Contract;
using isdayoff.Core;
using isdayoff.Core.Responses;

namespace isdayoff.Tests._Fakes
{
    internal class IsDayOffApiClientStub : IIsDayOffApiClient
    {
        public string Response { get; set; } = "0";

        public Task<GetDataApiResponse> GetDataAsync(
            DateTime from,
            DateTime to,
            Country country,
            Region? region,
            bool useShortDays, 
            bool treatNonWorkingDaysByCovidAsWorkingDayAdvanced, 
            bool useSixDaysWorkWeek,
            CancellationToken cancellationToken)
        {
            return Task.FromResult(new GetDataApiResponse(Response));
        }
    }
}