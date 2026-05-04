using System;

namespace PT.Tools.Http
{
    public class ApiException : Exception
    {
        public long StatusCode { get; }

        public ApiException(string message, long statusCode = 400) : base(message)
        {
            StatusCode = statusCode;
        }   
    }
}