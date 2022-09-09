#if (UNITY_EDITOR)


using System;
using UnityEngine;

namespace WakaTime
{
    [Serializable]
    public class HeartbeatResponse
    {
        public string id;
        public string entity;
        public string type;
        public float time;
    }

}


#endif
