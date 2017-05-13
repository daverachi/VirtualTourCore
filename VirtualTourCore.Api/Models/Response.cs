using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace VirtualTourCore.Api.Models
{
    public class Response<T>
    {
        public T Content { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public string Message { get; set; }

        public Response()
        {
            StatusCode = HttpStatusCode.BadRequest;
        }
    }
}