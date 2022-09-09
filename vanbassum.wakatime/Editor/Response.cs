#if (UNITY_EDITOR)

using System;
using System.Net;

namespace WakaTime
{

    [Serializable]
    public struct Response<T>
    {
        public HttpStatusCode StatusCode { get; set; }
        public string error;
        public T data;
    }

}


#endif
