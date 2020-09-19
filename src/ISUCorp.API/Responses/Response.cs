using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISUCorp.API.Responses
{
    public class Response<T>
    {
        public Response(string message)
        {
            Succeeded = false;
            Message = message;
            Errors = null;
            Data = default;
        }

        public Response(T data)
        {
            Succeeded = true;
            Message = string.Empty;
            Errors = null;
            Data = data;
        }

        public T Data { get; private set; }

        public bool Succeeded { get; private set; }

        public string[] Errors { get; private set; }

        public string Message { get; private set; }
    }
}
