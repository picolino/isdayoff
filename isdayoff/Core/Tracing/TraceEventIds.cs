namespace isdayoff.Core.Tracing
{
    public static class TraceEventIds
    {
        public static class Requesting
        {
            public const int REQUEST_SENDING = 100;
            public const int REQUEST_SENT = 102;
            public const int REQUEST_SENDING_ERROR = 101;
        }

        public static class Caching
        {
            public const int CACHE_SAVE_VALUE = 300;
            public const int CACHE_LOAD_VALUE_FOUND = 303;
            public const int CACHE_LOAD_VALUE_NOT_FOUND = 304;
        }
    }
}