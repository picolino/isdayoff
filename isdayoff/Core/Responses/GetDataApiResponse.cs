namespace isdayoff.Core.Responses
{
    internal class GetDataApiResponse
    {
        public GetDataApiResponse(string result)
        {
            Result = result;
        }

        public string Result { get; }
    }
}