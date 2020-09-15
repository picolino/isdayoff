using System;

namespace isdayoff.Core.Exceptions
{
    public class ServiceErrorException : Exception
    {
        public ServiceErrorException() : base("Something wrong with service.")
        {
        }
    }
}