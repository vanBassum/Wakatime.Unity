#if (UNITY_EDITOR)
using log4net.Repository.Hierarchy;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Timers;

namespace Wakatime
{
    public class WakatimeManager : IDisposable
    {
        private int SendInterval => 1000; //Maximum interval 1 second. Wakatime docs ask for max 10 requests per second.
        private Timer SendTimer { get; }
        private Queue<HeartbeatOld> HeartbeatQueue { get; }
        private HeartbeatCollector HeartbeatCollector { get; }
        private IWakatimeClient Client { get; }
        private Logger Logger { get; }
        public WakatimeManager(Logger logger, bool enabled, string projectName, string apiUri, string apiKey)
        {
            Logger = logger;

            if (!enabled)
            {
                logger.Log(LogLevels.Informational, "Wakatime disabled");
                return;
            }

            if (string.IsNullOrWhiteSpace(projectName))
            {
                logger.Log(LogLevels.Error, "projectName not found, please enter projectname in Window->vanBassum->Wakatime");
                return;
            }

            if (string.IsNullOrWhiteSpace(apiUri))
            {
                logger.Log(LogLevels.Error, "Api url not found.");
                return;
            }

            if (string.IsNullOrWhiteSpace(apiKey))
            {
                logger.Log(LogLevels.Error, "apiKey not found, please enter apiKey in Window->vanBassum->Wakatime");
                return;
            }

            HeartbeatQueue = new Queue<HeartbeatOld>();

            switch (Settings.ClientOptions)
            {
                //case ClientOptions.WakatimeCLIClient:
                //    Client = new WakatimeCliClient(apiUri, apiKey);
                //    break;
                case ClientTypes.WakatimeAPIClient:
                default:
                    Client = new WakatimeApiClient(apiUri, apiKey);
                    break;
            }


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

        void AddToQueue(HeartbeatOld heartbeat)
        {
            //Todo, ensure the queue doens't overflow.
            HeartbeatQueue.Enqueue(heartbeat);
            Logger.Log(LogLevels.Debug, $"Enqueue heartbeat, {HeartbeatQueue.Count} in queue");
        }

        private async Task SendQeueuAsync()
        {
            //Response<HeartbeatResponse> response = null;
            //List<HeartbeatOld> heartbeats = new List<HeartbeatOld>();
            //
            //while (heartbeats.Count < 25 && HeartbeatQueue.Any())
            //{
            //    heartbeats.Add(HeartbeatQueue.Dequeue());
            //}
            //
            //if (heartbeats.Count == 0)
            //    return;
            //
            //
            //if (heartbeats.Count == 1)
            //    response = await Client.SendHeartbeat(heartbeats.First());
            //
            //if (heartbeats.Count > 1)
            //    response = await Client.SendHeartbeats(heartbeats);
            //
            //bool success = HandleResponseCodes(response);
            //
            //if(success)
            //{
            //    Logger.Log(LogLevels.Informational, heartbeats.Count.ToString() + " Heartbeats send successfully");
            //    if(SendTimer.Interval != SendInterval)
            //    {
            //        SendTimer.Interval = SendInterval;
            //        Logger.Log(LogLevels.Debug, $"Send interval returned to {SendTimer.Interval}");
            //    }
            //}
            //else
            //{
            //    Logger.Log(LogLevels.Warning, heartbeats.Count.ToString() + " Heartbeats failed to send, will retry later");
            //    //somehow sending failed, try again later.
            //    foreach (var heartbeat in heartbeats)
            //        AddToQueue(heartbeat);
            //}
        }

        private bool HandleResponseCodes<T>(Response<T> response)
        {
            if(response == null)
                return false;
            switch (response?.StatusCode)
            {
                case HttpStatusCode.Accepted:
                case HttpStatusCode.Created:
                    return true;

                case (HttpStatusCode)429:
                    SendTimer.Interval += 1000;             //Increase interval.
                    Logger.Log(LogLevels.Warning, $"Response error {response?.StatusCode.ToString() ?? "N/A"}. Too many requests send, increasing interval to: {SendTimer.Interval}");
                    return false;

                default:
                    Logger.Log(LogLevels.Warning, $"Response error {response?.StatusCode.ToString()}: {response?.error}");
                    return false;
            }
        }

        public void Dispose()
        {
            HeartbeatCollector?.Dispose();
            SendTimer?.Dispose();
            Logger.Log(LogLevels.Warning, "Plugin stopped");
        }
    }
}


#endif
