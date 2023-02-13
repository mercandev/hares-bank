using System;
using System.Net;
using Newtonsoft.Json;

namespace HB.Infrastructure.Exceptions
{
    [Serializable]
    public class CustomExceptionResponse
	{
        public HttpStatusCode Status { get; set; }
        public object? Data { get; set; }
        public string? ErrorMessage { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}

