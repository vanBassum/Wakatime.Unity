#if (UNITY_EDITOR)
using System;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using Wakatime;

namespace Wakatime
{
    [InitializeOnLoad]
    public class Plugin
    {
        public static Logger Logger { get; set; }
        public static IHeartbeatCollector Collector { get; set; }
        public static IWakatimeHandler Handler { get; set; }
        public static Settings Settings { get; set; }
        public static SettingsManager SettingsManager { get; set; }


        static Plugin()
        {
            Initialize();
        }

        public static void Initialize()
        {
            Dispose();
            try
            {
                Logger = new Logger("Wakatime", Settings.LogLevel);
                SettingsManager = new SettingsManager(Logger);
                Settings = SettingsManager.LoadSettings();
                Collector = new HeartbeatCollector(Logger, Settings);   //Only 1 type, no switch case required

                switch (Settings.WakatimeHandlerType)
                {
                    case WakatimeHandlerTypes.WakatimeCli:
                        Handler = new WakatimeCliHandler(Logger, Settings);
                        break;
                }

                Collector.OnHeartbeat += (sender, e) => Handler.HandleHeartBeat(e);

                Logger.Log(LogLevels.Notice, $"Plugin starting for project '{Settings.ProjectName}'");
            }
            catch(Exception ex)
            {
                Debug.LogException(ex);
            }
        }

        public static void Dispose()
        {
            Logger?.Dispose();
            Collector?.Dispose();
            Handler?.Dispose();
            Settings?.Dispose();
            SettingsManager?.Dispose();
        }


        [DidReloadScripts]
        static void OnScriptReload()
        {
            Initialize();
        }
    }





}


#endif
