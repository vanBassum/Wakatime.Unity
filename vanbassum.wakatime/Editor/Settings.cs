#if (UNITY_EDITOR)
using UnityEditor;
using UnityEngine;

namespace WakaTime
{
    public static class Settings
    {
        public static bool Enabled
        {
            get => EditorPrefs.GetBool("WakaTime/Enabled", false);
            set => EditorPrefs.SetBool("WakaTime/Enabled", value);
        }

        public static string ApiKey
        {
            get => EditorPrefs.GetString("WakaTime/APIKey", "");
            set => EditorPrefs.SetString("WakaTime/APIKey", value);
        }

        public static string ProjectName
        {
            get => EditorPrefs.GetString("WakaTime/Project/Name", Application.productName);
            set => EditorPrefs.SetString("WakaTime/Project/Name", value);
        }

        public static Logger.Levels LogLevel
        {
            get => (Logger.Levels)EditorPrefs.GetInt("WakaTime/LogLevel", (int)Logger.Levels.Informational);
            set => EditorPrefs.SetInt("WakaTime/LogLevel", (int)value);
        }

        public static string ApiUri => "https://api.wakatime.com/api/v1/";

    }
}


#endif
