using System.Net;

namespace HamatetsuScheduler.Api.Exceptions
{
    public class ResponseException: Exception
    {
        public HttpStatusCode StatusCode { get; }
        public object Failure { get; }
        public ResponseException(HttpStatusCode statusCode, string message)
            : base(message)
        {
            StatusCode = statusCode;

            Failure = new 
            {
                Status = "error",
                Message = "Validation failed",
                Errors = message
            };
        }
    }
}
