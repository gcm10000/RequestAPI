using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace RequestAPI
{
    public class DataReturned<T>
    {
        public HttpStatusCode StatusCode { get; }
        public long Timer { get; }
        public int Size { get; }
        public T Body { get; }
        public DataReturned(HttpStatusCode statusCode, long timer, int size, T body)
        {
            StatusCode = statusCode;
            Timer = timer;
            Size = size;
            Body = body;
        }
    }
}
