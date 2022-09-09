#if (UNITY_EDITOR)
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace WakaTime
{
    public class WakatimeApiClient
    {
        private string ApiKey { get; }
        private string ApiUri { get; }

        public WakatimeApiClient(string apiUri, string apiKey)
        {
            ApiKey = apiKey;
            ApiUri = apiUri;
        }

        public async Task<Response<HeartbeatResponse>> SendHearthbeat(Heartbeat heartbeat)
        {
            return await Post<HeartbeatResponse, Heartbeat>("users/current/heartbeats", heartbeat);
        }

        public async Task<Response<Tres>> Post<Tres, Treq>(string path, Treq heartbeat)
        {
            string endpoint = ApiUri + $"{path}?api_key={ApiKey}";
            using HttpClient client = new HttpClient();
            var jsonMessage = JsonUtility.ToJson(heartbeat);
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, endpoint);
            requestMessage.Content = new StringContent(jsonMessage, Encoding.UTF8, "application/json");
            var response = await client.SendAsync(requestMessage);
            var result = new Response<Tres>();
            var responseMessage = await response.Content.ReadAsStringAsync();
            if (!String.IsNullOrWhiteSpace(responseMessage))
                result = JsonUtility.FromJson<Response<Tres>>(responseMessage);
            result.StatusCode = response.StatusCode;
            return result;
        }
    }
}


#endif
