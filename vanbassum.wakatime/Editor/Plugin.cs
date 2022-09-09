#if (UNITY_EDITOR)
using System.IO;
using UnityEditor;

namespace WakaTime
{
    [InitializeOnLoad]
    public class Plugin
    {
        static Logger logger;
        static WakatimeManager manager;
        static Plugin()
        {
            
            Initialize();
        }

        public static void Initialize()
        {
            logger = new Logger("Wakatime", Settings.LogLevel);
            logger.Log(Logger.Levels.Informational, "Plugin initializing");
            manager = new WakatimeManager(
                logger,
                Settings.Enabled,
                Settings.ProjectName,
                Settings.ApiUri,
                Settings.ApiKey);
        }
    }
}


#endif
