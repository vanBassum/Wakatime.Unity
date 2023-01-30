#if (UNITY_EDITOR)
using Unity.Plastic.Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.Threading;

namespace WakaTime
{
    public class WakatimeCliClient : IWakatimeClient
    {
        private string ApiKey { get; }
        private string ApiUri { get; }

        public WakatimeCliClient(string apiUri, string apiKey)
        {
            ApiKey = apiKey;
            ApiUri = apiUri;
            var os = SystemInfo.operatingSystemFamily.ToString();
        }

        public async Task<Response<HeartbeatResponse>> SendHeartbeat(Heartbeat heartbeat, CancellationToken cancellationToken = default)
        {
            return await Post<HeartbeatResponse, Heartbeat>("users/current/heartbeats", heartbeat, cancellationToken);
        }

        public async Task<Response<HeartbeatResponse>> SendHeartbeats(List<Heartbeat> heartbeats, CancellationToken cancellationToken = default)
        {
            //Wakatime docs say, max 25 per request
            if (heartbeats.Count > 25)
                return new Response<HeartbeatResponse>() { StatusCode = HttpStatusCode.BadRequest };
            return await Post<HeartbeatResponse, List<Heartbeat>>("users/current/heartbeats.bulk", heartbeats, cancellationToken);
        }


        private async Task<Response<Tres>> Post<Tres, Treq>(string path, Treq heartbeat, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException("implement using wakatime CLI");
        }
    }
}


#endif
