#if (UNITY_EDITOR)
using Unity.Plastic.Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Networking;
using UnityEngine;
using System.Net.Http.Headers;
using System.Threading;

namespace Wakatime
{

    [ObsoleteAttribute("This client is obsolete, use the cliClient instead", false)]
    public class WakatimeApiClient : IWakatimeClient
    {
        private static readonly HttpClient client = new HttpClient();
        private string ApiKey { get; }
        private string ApiUri { get; }

        public WakatimeApiClient(string apiUri, string apiKey)
        {
            ApiKey = apiKey;
            ApiUri = apiUri;

            var os = SystemInfo.operatingSystemFamily.ToString();

            var userAgent = $"UnityPlayer/{Application.unityVersion} ({os}) unity/1.0 unity-wakatime/1.0.0";
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.UserAgent.Clear();
            client.DefaultRequestHeaders.UserAgent.ParseAdd(userAgent);
        }

        public async Task<Response<HeartbeatResponse>> SendHeartbeat(HeartbeatOld heartbeat, CancellationToken cancellationToken = default)
        {
            return await Post<HeartbeatResponse, HeartbeatOld>("users/current/heartbeats", heartbeat, cancellationToken);
        }

        public async Task<Response<HeartbeatResponse>> SendHeartbeats(List<HeartbeatOld> heartbeats, CancellationToken cancellationToken = default)
        {
            //Wakatime docs say, max 25 per request
            if (heartbeats.Count > 25)
                return new Response<HeartbeatResponse>() { StatusCode = HttpStatusCode.BadRequest };
            return await Post<HeartbeatResponse, List<HeartbeatOld>>("users/current/heartbeats.bulk", heartbeats, cancellationToken);
        }


        private async Task<Response<Tres>> Post<Tres, Treq>(string path, Treq heartbeat, CancellationToken cancellationToken = default)
        {
            string endpoint = ApiUri + $"{path}?api_key={ApiKey}";                
            var jsonMessage = JsonConvert.SerializeObject(heartbeat);
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, endpoint);
            requestMessage.Content = new StringContent(jsonMessage, Encoding.UTF8, "application/json");
            var response = await client.SendAsync(requestMessage, cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();
            var result = new Response<Tres>();
            var responseMessage = await response.Content.ReadAsStringAsync();
            if (!String.IsNullOrWhiteSpace(responseMessage))
                result = JsonConvert.DeserializeObject<Response<Tres>>(responseMessage);
            result.StatusCode = response.StatusCode;
            return result;
        }

        public Task<Response<HeartbeatResponse>> SendHeartbeat(Heartbeat heartbeat, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Response<HeartbeatResponse>> SendHeartbeats(List<Heartbeat> heartbeats, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}


#endif
