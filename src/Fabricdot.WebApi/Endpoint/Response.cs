namespace Fabricdot.WebApi.Endpoint
{
    public class Response<T>
    {
        public int Code { get; set; }

        public bool Success { get; set; }

        public string? Message { get; set; }

        public T? Data { get; set; }

        public Response()
        {
        }

        public Response(T? data)
        {
            Data = data;
        }

        public Response(
            string? message,
            int code)
        {
            Message = message;
            Code = code;
        }
    }
}