#if (UNITY_EDITOR)
using System;
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
            get => Application.productName;
        }

        public static Logger.Levels LogLevel
        {
            get => (Logger.Levels)EditorPrefs.GetInt("WakaTime/LogLevel", (int)Logger.Levels.Notice);
            set => EditorPrefs.SetInt("WakaTime/LogLevel", (int)value);
        }

        public static string ApiUri => "https://api.wakatime.com/api/v1/";

        public static TimeSpan SameFileTimeout => TimeSpan.FromMinutes(2);

    }
}


#endif
