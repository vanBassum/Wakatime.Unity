#if (UNITY_EDITOR)
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEngine;

namespace Wakatime
{
    public class SettingsWindow : EditorWindow
    {
        bool enabled;
        string apiKey;
        string projectName;
        ClientTypes clientOptions;
        GitClientTypes gitOptions;
        LogLevels logLevel;

        [MenuItem("Services/vanbassum/WakaTime")]
        static void Init()
        {
            SettingsWindow window = (SettingsWindow)GetWindow(typeof(SettingsWindow), false, "Wakatime settings");
            window.LoadSettings();
            window.Show();
        }

        public void LoadSettings()
        {
            enabled = Settings.Enabled;
            apiKey = Settings.ApiKey;
            projectName = Settings.ProjectName;
            logLevel = Settings.LogLevel;
            gitOptions = Settings.GitOptions;
        }


        void OnGUI()
        {
            enabled = EditorGUILayout.Toggle("Enable WakaTime", enabled);
            //projectName = EditorGUILayout.TextField("Project name", projectName);
            logLevel = (LogLevels)EditorGUILayout.EnumPopup("Log level", logLevel);

            EditorGUILayout.BeginHorizontal();
            apiKey = EditorGUILayout.TextField("API key", apiKey);
            if (GUILayout.Button("Get key"))
                Application.OpenURL("https://wakatime.com/api-key");
            EditorGUILayout.EndHorizontal();

            gitOptions = (GitClientTypes)EditorGUILayout.EnumPopup("Git options", gitOptions);
            clientOptions = (ClientTypes)EditorGUILayout.EnumPopup("Client options", clientOptions);

            if (GUILayout.Button("Open dashboard"))
                Application.OpenURL("https://wakatime.com/dashboard");

            
            EditorGUILayout.BeginHorizontal();
            bool save = GUILayout.Button("Save preferences");
            bool cancel = GUILayout.Button("Cancel");
            EditorGUILayout.EndHorizontal();

            if (save)
            {
                Settings.Enabled = enabled;
                Settings.ApiKey = apiKey;
                Settings.LogLevel = logLevel;
                Settings.GitOptions = gitOptions;
                //Settings.ProjectName = projectName;
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


#endif
