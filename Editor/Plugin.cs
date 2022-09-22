#if (UNITY_EDITOR)
using System;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace WakaTime
{
    [InitializeOnLoad]
    public class Plugin
    {
        public static Logger Logger { get; set; }
        public static WakatimeManager Manager { get; set; }
        static Plugin()
        {
            Initialize();
        }

        public static void Initialize()
        {
            Manager?.Dispose();
            Logger?.Dispose();
            try
            {
                Logger = new Logger("Wakatime", Settings.LogLevel);
                Logger.Log(Logger.Levels.Notice, $"Plugin starting for project '{Settings.ProjectName}'");
                Manager = new WakatimeManager(
                    Logger,
                    Settings.Enabled,
                    Settings.ProjectName,
                    Settings.ApiUri,
                    Settings.ApiKey);
            }
            catch(Exception ex)
            {
                Debug.LogException(ex);
            }

        }

        [DidReloadScripts]
        static void OnScriptReload()
        {
            Initialize();
        }
    }
}


#endif
