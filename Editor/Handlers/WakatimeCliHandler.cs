#if (UNITY_EDITOR)

using System;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Timers;
using UnityEditor.SearchService;

namespace Wakatime
{

    public class WakatimeCliHandler : IWakatimeHandler
    {
        private Logger Logger { get; set; }
        private Settings Settings { get; set; }

        private readonly Timer _timer;
        public readonly ConcurrentQueue<Heartbeat> HeartbeatQueue;
        private readonly CliParameters _cliParameters;
        private string _lastFile;
        private DateTime _lastHeartbeat;

        public WakatimeCliHandler(Logger logger, Settings settings)
        {
            Logger= logger;
            Settings= settings;

            HeartbeatQueue = new ConcurrentQueue<Heartbeat>();
            _cliParameters = new CliParameters
            {
                Key = settings.ApiKey,
                //Plugin = $"{_metadata.EditorName}/{_metadata.EditorVersion} {_metadata.PluginName}/{_metadata.PluginVersion}"
            };

            _timer = new Timer(10000);
            _timer.Elapsed += (s, e) => ProcessHeartbeats();
            _timer.Start();
        }



        private bool EnoughTimePassed(DateTime now)
        {
            return now > _lastHeartbeat + Settings.HeartbeatFrequency;
        }

        public void HandleHeartBeat(Heartbeat heartbeat)
        {
            var currentFile = heartbeat.Entity;
            if (currentFile == null)
                return;

            var now = DateTime.UtcNow;

            if (!heartbeat.IsWrite && _lastFile != null && !EnoughTimePassed(now) && currentFile.Equals(_lastFile))
                return;

            _lastFile = currentFile;
            _lastHeartbeat = now;

            HeartbeatQueue.Enqueue(heartbeat);
        }

        private void ProcessHeartbeats()
        {
            try
            {
                var binary = Settings.WakatimeCliBinary;

                // get first heartbeat from queue
                var gotOne = HeartbeatQueue.TryDequeue(out var heartbeat);
                if (!gotOne)
                    return;

                // pop all extra heartbeats from queue
                var extraHeartbeats = new Collection<Heartbeat>();
                while (HeartbeatQueue.TryDequeue(out var h))
                    extraHeartbeats.Add(h);

                var hasExtraHeartbeats = extraHeartbeats.Count > 0;

                _cliParameters.File = heartbeat.Entity;
                _cliParameters.Time = heartbeat.Timestamp;
                _cliParameters.IsWrite = heartbeat.IsWrite;
                _cliParameters.Project = heartbeat.Project;
                _cliParameters.Category = heartbeat.Category;
                _cliParameters.EntityType = heartbeat.EntityType;
                _cliParameters.HasExtraHeartbeats = hasExtraHeartbeats;

                string extraHeartbeatsString = null;
                if (hasExtraHeartbeats)
                    extraHeartbeatsString = JSONSerializer.SerializeArrayHeartbeat(extraHeartbeats);

                var process = new ProcessRunner(Logger, binary, _cliParameters.ToArray());

                if (Settings.LogLevel == LogLevels.Debug)
                {
                    Logger.Log(LogLevels.Debug,
                        $"[\"{binary}\", \"{string.Join("\", \"", _cliParameters.ToArray(true))}\"]");

                    process.Run(extraHeartbeatsString);

                    if (!string.IsNullOrEmpty(process.Output))
                        Logger.Log(LogLevels.Debug, process.Output);

                    if (!string.IsNullOrEmpty(process.Error))
                        Logger.Log(LogLevels.Debug, process.Error);
                }
                else
                    process.RunInBackground(extraHeartbeatsString);

                if (process.Success) return;

                Logger.Log(LogLevels.Error, "Could not send heartbeat.");

                if (!string.IsNullOrEmpty(process.Output))
                    Logger.Log(LogLevels.Error, process.Output);

                if (!string.IsNullOrEmpty(process.Error))
                    Logger.Log(LogLevels.Error, process.Error);
            }
            catch (Exception ex)
            {
                Logger.Log(LogLevels.Error, "Error processing heartbeat(s).", ex.Message);
            }
        }

        public void Dispose()
        {
            if (_timer != null)
            {
                _timer.Stop();
                _timer.Dispose();
            }

            // make sure the queue is empty	
            ProcessHeartbeats();
        }





    }
}


#endif
