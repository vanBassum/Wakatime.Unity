#if (UNITY_EDITOR)
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEngine;

namespace WakaTime
{
    public class UI_Settings : EditorWindow
    {
        bool enabled;
        string apiKey;
        string projectName;
        GitOptions gitOptions;
        Logger.Levels logLevel;

        [MenuItem("Services/vanbassum/WakaTime")]
        static void Init()
        {
            UI_Settings window = (UI_Settings)GetWindow(typeof(UI_Settings), false, "Wakatime settings");
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
            logLevel = (Logger.Levels)EditorGUILayout.EnumPopup("Log level", logLevel);

            EditorGUILayout.BeginHorizontal();
            apiKey = EditorGUILayout.TextField("API key", apiKey);
            if (GUILayout.Button("Get key"))
                Application.OpenURL("https://wakatime.com/api-key");
            EditorGUILayout.EndHorizontal();

            gitOptions = (GitOptions)EditorGUILayout.EnumPopup("Git options", gitOptions);

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
