#if (UNITY_EDITOR)

using System;
using System.Net;

namespace Wakatime
{

    [Serializable]
    public class Response<T>
    {
        public HttpStatusCode StatusCode { get; set; }
        public string error;
        public T data;
    }
}


#endif
