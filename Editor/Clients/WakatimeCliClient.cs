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
using System.Security.Cryptography;
using UnityEngine.Profiling.Memory.Experimental;
using System.Linq;
using Newtonsoft.Json;
using Codice.Client.BaseCommands.BranchExplorer.ExplorerTree;

namespace Wakatime
{
    public class WakatimeCliClient : IWakatimeClient
    {
        private CliParameters CliParameters { get; }
        private Logger Logger { get; set; }
        public WakatimeCliClient(Logger logger, string apiUri, string apiKey)
        {
            CliParameters = new CliParameters
            {
                Key = apiKey,
                //Plugin = $"{_metadata.EditorName}/{_metadata.EditorVersion} {_metadata.PluginName}/{_metadata.PluginVersion}"
            };

            var os = SystemInfo.operatingSystemFamily.ToString();
            Logger = logger;
        }

        public async Task<Response<HeartbeatResponse>> SendHeartbeat(Heartbeat heartbeat, CancellationToken cancellationToken = default)
        {
            return await PostAsync(heartbeat, new List<Heartbeat>(), cancellationToken);
        }

        public async Task<Response<HeartbeatResponse>> SendHeartbeats(List<Heartbeat> heartbeats, CancellationToken cancellationToken = default)
        {
            //Wakatime docs say, max 25 per request
            if (heartbeats.Count > 25)
                return new Response<HeartbeatResponse>() { StatusCode = HttpStatusCode.BadRequest };
            if (heartbeats.Count <= 0)
                return null;

            Heartbeat firstHeartbeat = heartbeats.FirstOrDefault();
            heartbeats.Remove(firstHeartbeat);
            return await PostAsync(firstHeartbeat, heartbeats, cancellationToken);
        }


        private async Task<Response<HeartbeatResponse>> PostAsync(Heartbeat firstHearthbeat, List<Heartbeat> extraHeartbeats, CancellationToken cancellationToken = default)
        {
            var binary = Settings.WakatimeCliBinary;
            var hasExtraHeartbeats = extraHeartbeats.Count > 0;

            CliParameters.File = firstHearthbeat.Entity;
            CliParameters.Time = firstHearthbeat.Timestamp;
            CliParameters.IsWrite = firstHearthbeat.IsWrite;
            CliParameters.Project = firstHearthbeat.Project;
            CliParameters.Category = firstHearthbeat.Category;
            CliParameters.EntityType = firstHearthbeat.EntityType;
            CliParameters.HasExtraHeartbeats = hasExtraHeartbeats;

            string extraHeartbeatsString = null;
            if (hasExtraHeartbeats)
                extraHeartbeatsString = JSONSerializer.SerializeArrayHeartbeat(extraHeartbeats);


            var process = new ProcessRunner(Logger, binary, CliParameters.ToArray());


            if(Settings.LogLevel == LogLevels.Debug)
            {
                Logger.Log(LogLevels.Debug, $"[\"{binary}\", \"{string.Join("\", \"", CliParameters.ToArray(true))}\"]");
                process.Run(extraHeartbeatsString);

                if (!string.IsNullOrEmpty(process.Output))
                    Logger.Log(LogLevels.Debug, process.Output);

                if (!string.IsNullOrEmpty(process.Error))
                    Logger.Log(LogLevels.Debug, process.Error);
            }
            else
                process.RunInBackground(extraHeartbeatsString);

            Logger.Log(LogLevels.Error, "Could not send heartbeat.");

            if (!string.IsNullOrEmpty(process.Output))
                Logger.Log(LogLevels.Error, process.Output);

            if (!string.IsNullOrEmpty(process.Error))
                Logger.Log(LogLevels.Error, process.Error);

            throw new Exception();
        }
    }
}


#endif
