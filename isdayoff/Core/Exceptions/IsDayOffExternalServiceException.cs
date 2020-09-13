using System;

namespace isdayoff.Core.Exceptions
{
    public class IsDayOffExternalServiceException : Exception
    {
        public IsDayOffExternalServiceException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}