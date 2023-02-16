using System;
using System.Net;
using Newtonsoft.Json;

namespace HB.SharedObject
{
    [Serializable]
    public class ReturnState<T>
	{
		public ReturnState(object result)
		{
			Status = !string.IsNullOrWhiteSpace(ErrorMessage)
				? HttpStatusCode.NotAcceptable : HttpStatusCode.OK;

			Data = (T?)result;
		}

		public ReturnState(HttpStatusCode statusCode , string? errorMessage = null, object? data = null)
		{
			Status = statusCode;
			ErrorMessage = errorMessage;
			Data = (T?)data;
		}

		public HttpStatusCode Status { get; set; }
		public T Data { get; set; }
		public string ErrorMessage { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}

