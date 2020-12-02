using System;

namespace isdayoff.Core.Exceptions
{
    public class IsDayOffExternalServiceException : Exception
    {
        public IsDayOffExternalServiceException(Exception innerException) 
            : base(ErrorsMessages.ExternalServiceDidNotHandleTheRequestSeeInnerException(), innerException)
        {
        }
    }
}