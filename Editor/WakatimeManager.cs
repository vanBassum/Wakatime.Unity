#if (UNITY_EDITOR)
using log4net.Repository.Hierarchy;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Timers;

namespace WakaTime
{
    public class WakatimeManager : IDisposable
    {
        private int SendInterval => 1000; //Maximum interval 1 second. Wakatime docs ask for max 10 requests per second.
        private Timer SendTimer { get; }
        private Queue<Heartbeat> HeartbeatQueue { get; }
        private HeartbeatCollector HeartbeatCollector { get; }
        private WakatimeApiClient Client { get; }
        private Logger Logger { get; }
        public WakatimeManager(Logger logger, bool enabled, string projectName, string apiUri, string apiKey)
        {
            Logger = logger;

            if (!enabled)
            {
                logger.Log(Logger.Levels.Informational, "Wakatime disabled");
                return;
            }

            if (string.IsNullOrWhiteSpace(projectName))
            {
                logger.Log(Logger.Levels.Error, "projectName not found, please enter projectname in Window->vanBassum->Wakatime");
                return;
            }

            if (string.IsNullOrWhiteSpace(apiUri))
            {
                logger.Log(Logger.Levels.Error, "Api url not found.");
                return;
            }

            if (string.IsNullOrWhiteSpace(apiKey))
            {
                logger.Log(Logger.Levels.Error, "apiKey not found, please enter apiKey in Window->vanBassum->Wakatime");
                return;
            }

            HeartbeatQueue = new Queue<Heartbeat>();
            Client = new WakatimeApiClient(apiUri, apiKey);

            HeartbeatCollector = new HeartbeatCollector(logger, projectName);
            HeartbeatCollector.OnHeartbeat += (sender, e) => AddToQueue(e); 

            SendTimer = new Timer();
            SendTimer.Interval = SendInterval;
            SendTimer.Elapsed += SendTimer_Elapsed;
            SendTimer.AutoReset = true;
            SendTimer.Start();
        }

        private async void SendTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            SendTimer.Stop();
            //Logger.Log(Logger.Levels.Debug, "Send timer elapsed");
            await SendQeueuAsync();
            SendTimer.Start();
        }

        void AddToQueue(Heartbeat heartbeat)
        {
            //Todo, ensure the queue doens't overflow.
            HeartbeatQueue.Enqueue(heartbeat);
            Logger.Log(Logger.Levels.Debug, $"Enqueue heartbeat, {HeartbeatQueue.Count} in queue");
        }

        private async Task SendQeueuAsync()
        {
            Response<HeartbeatResponse> response = null;
            List<Heartbeat> heartbeats = new List<Heartbeat>();

            while (heartbeats.Count < 25 && HeartbeatQueue.Any())
            {
                heartbeats.Add(HeartbeatQueue.Dequeue());
            }

            if (heartbeats.Count == 0)
                return;

            if (heartbeats.Count == 1)
                response = await Client.SendHeartbeat(heartbeats.First());
       
            if (heartbeats.Count > 1)
                response = await Client.SendHeartbeats(heartbeats);

            bool success = HandleResponseCodes(response);

            if(success)
            {
                Logger.Log(Logger.Levels.Informational, heartbeats.Count.ToString() + " Heartbeats send successfully");
                if(SendTimer.Interval != SendInterval)
                {
                    SendTimer.Interval = SendInterval;
                    Logger.Log(Logger.Levels.Debug, $"Send interval returned to {SendTimer.Interval}");
                }
            }
            else
            {
                Logger.Log(Logger.Levels.Warning, heartbeats.Count.ToString() + " Heartbeats failed to send, will retry later");
                //somehow sending failed, try again later.
                foreach (var heartbeat in heartbeats)
                    AddToQueue(heartbeat);
            }
        }

        private bool HandleResponseCodes<T>(Response<T> response)
        {
            switch (response?.StatusCode)
            {
                case HttpStatusCode.Accepted:
                case HttpStatusCode.Created:
                    return true;

                case (HttpStatusCode)429:
                    SendTimer.Interval += 1000;             //Increase interval.
                    Logger.Log(Logger.Levels.Warning, $"Response error {response?.StatusCode.ToString() ?? "N/A"}. Too many requests send, increasing interval to: {SendTimer.Interval}");
                    return false;

                default:
                    Logger.Log(Logger.Levels.Warning, $"Response error {response?.StatusCode.ToString()}: {response?.error}");
                    return false;
            }
        }

        public void Dispose()
        {
            HeartbeatCollector?.Dispose();
            SendTimer?.Dispose();
            Logger.Log(Logger.Levels.Warning, "Plugin stopped");
        }
    }
}


#endif
