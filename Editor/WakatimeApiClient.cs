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

namespace WakaTime
{
    public class WakatimeApiClient
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

        public async Task<Response<HeartbeatResponse>> SendHeartbeat(Heartbeat heartbeat)
        {
            return await Post<HeartbeatResponse, Heartbeat>("users/current/heartbeats", heartbeat);
        }

        public async Task<Response<HeartbeatResponse>> SendHeartbeats(List<Heartbeat> heartbeats)
        {
            //Wakatime docs say, max 25 per request
            if (heartbeats.Count > 25)
                return new Response<HeartbeatResponse>() { StatusCode = HttpStatusCode.BadRequest };
            return await Post<HeartbeatResponse, List<Heartbeat>>("users/current/heartbeats.bulk", heartbeats);
        }


        public async Task<Response<Tres>> Post<Tres, Treq>(string path, Treq heartbeat)
        {
            string endpoint = ApiUri + $"{path}?api_key={ApiKey}";                
            var jsonMessage = JsonConvert.SerializeObject(heartbeat);
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, endpoint);
            requestMessage.Content = new StringContent(jsonMessage, Encoding.UTF8, "application/json");
            var response = await client.SendAsync(requestMessage);
            var result = new Response<Tres>();
            var responseMessage = await response.Content.ReadAsStringAsync();
            if (!String.IsNullOrWhiteSpace(responseMessage))
                result = JsonConvert.DeserializeObject<Response<Tres>>(responseMessage);
            result.StatusCode = response.StatusCode;
            return result;
        }
    }
}


#endif
