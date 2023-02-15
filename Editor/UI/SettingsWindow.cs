#if (UNITY_EDITOR)
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEngine;

namespace Wakatime
{
    public class SettingsWindow : EditorWindow
    {
        static Settings settings;
        [MenuItem("Services/vanbassum/WakaTime")]
        static void Init()
        {
            SettingsWindow window = (SettingsWindow)GetWindow(typeof(SettingsWindow), false, "Wakatime settings");
            if (Plugin.Settings == null)
                Plugin.Initialize();
            settings = (Settings)Plugin.Settings.Clone();
            window.Show();
        }


        void OnGUI()
        {
            if(settings != null)
            {
                settings.Enabled = EditorGUILayout.Toggle("Enable WakaTime", settings.Enabled);
                //projectName = EditorGUILayout.TextField("Project name", projectName);
                settings.LogLevel = (LogLevels)EditorGUILayout.EnumPopup("Log level", settings.LogLevel);

                EditorGUILayout.BeginHorizontal();
                settings.ApiKey = EditorGUILayout.TextField("API key", settings.ApiKey);
                if (GUILayout.Button("Get key"))
                    Application.OpenURL("https://wakatime.com/api-key");
                EditorGUILayout.EndHorizontal();

                settings.GitOptions = (GitClientTypes)EditorGUILayout.EnumPopup("Git options", settings.GitOptions);
                //settings.ClientOptions = (ClientTypes)EditorGUILayout.EnumPopup("Client options", settings.ClientOptions);

                if (GUILayout.Button("Open dashboard"))
                    Application.OpenURL("https://wakatime.com/dashboard");


                EditorGUILayout.BeginHorizontal();
                bool save = GUILayout.Button("Save preferences");
                bool cancel = GUILayout.Button("Cancel");
                EditorGUILayout.EndHorizontal();

                if (save)
                {
                    Plugin.SettingsManager.SaveSettings(settings);
                    Plugin.Initialize();
                    this.Close();
                }
                if (cancel)
                {
                    this.Close();
                }
            }
        }
    }
}


#endif
