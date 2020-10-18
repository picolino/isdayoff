﻿using System;
using System.Threading;
using System.Threading.Tasks;
using isdayoff.Contract;
using isdayoff.Core.Responses;

namespace isdayoff.Core
{
    internal interface IIsDayOffApiClient
    {
        Task<GetDataApiResponse> GetDataAsync(DateTime from, DateTime to, Country country, CancellationToken cancellationToken);
    }
}