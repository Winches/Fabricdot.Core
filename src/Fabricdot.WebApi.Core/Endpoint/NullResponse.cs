namespace Fabricdot.WebApi.Core.Endpoint
{
    public class NullResponse : Response<object>
    {
        private NullResponse()
        {
        }

        public static NullResponse Null => new NullResponse();
    }
}