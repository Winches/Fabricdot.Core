namespace Fabricdot.WebApi.Endpoint
{
    public class NullResponse : Response<object>
    {
        private NullResponse()
        {
        }

        public static NullResponse Null => new NullResponse();
    }
}