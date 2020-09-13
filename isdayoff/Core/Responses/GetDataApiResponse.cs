namespace isdayoff.Core.Responses
{
    public class GetDataApiResponse
    {
        public GetDataApiResponse(string result)
        {
            Result = result;
        }

        public string Result { get; }
    }
}