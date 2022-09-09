#if (UNITY_EDITOR)
using System.IO;
using UnityEditor;

namespace WakaTime
{
    [InitializeOnLoad]
    public class Plugin
    {
        static WakatimeManager manager;
        static Plugin()
        {
            Initialize();
        }

        public static void Initialize()
        {
            manager?.Stop();

            manager = new WakatimeManager(
                Settings.Enabled,
                Settings.ProjectName,
                Settings.ApiUri,
                Settings.ApiKey);

            //manager.Start();
        }

        public static void Test()
        {
            var manager = new WakatimeManager(
                Settings.Enabled,
                Settings.ProjectName,
                Settings.ApiUri,
                Settings.ApiKey);

            manager.Start();
        }
    }

}


#endif
