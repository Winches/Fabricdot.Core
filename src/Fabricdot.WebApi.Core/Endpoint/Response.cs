namespace Fabricdot.WebApi.Core.Endpoint
{
    public class Response<T>
    {
        public int Code { get; set; }

        public bool Success => Code == 0;

        public string Message { get; set; }

        public T Data { get; set; }

        public Response()
        {
        }

        public Response(T data)
        {
            Data = data;
        }

        public Response(string message, int code)
        {
            Message = message;
            Code = code;
        }

        internal void SetUnExcepted(string message, int code = 911)
        {
            Data = default;
            Message = message;
            Code = code;
        }
    }
}