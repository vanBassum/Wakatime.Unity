#if (UNITY_EDITOR)
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

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
            using HttpClient client = new HttpClient();
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
