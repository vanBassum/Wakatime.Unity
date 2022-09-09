#if (UNITY_EDITOR)
using UnityEditor;
using UnityEngine;

namespace WakaTime
{
    public class UI_Settings : EditorWindow
    {
        [MenuItem("Window/vanbassum/WakaTime")]
        static void Init()
        {
            UI_Settings window = (UI_Settings)GetWindow(typeof(UI_Settings), false, Settings.ProjectName);
            window.Show();
        }


        void OnGUI()
        {
            bool enabled = EditorGUILayout.Toggle("Enable WakaTime", Settings.Enabled);
            string apiKey = EditorGUILayout.TextField("API key", Settings.ApiKey);
            string projectName = EditorGUILayout.TextField("Project name", Settings.ProjectName);

            EditorGUILayout.BeginHorizontal();
            bool save = GUILayout.Button("Save Preferences");
            bool cancel = GUILayout.Button("Cancel");
            EditorGUILayout.EndHorizontal();

            if (save)
            {
                Settings.Enabled = enabled;
                Settings.ApiKey = apiKey;
                Settings.ProjectName = projectName;
                Plugin.Initialize();
                this.Close();
            }

            if (cancel)
            { 
                this.Close();
            }

            if(GUILayout.Button("Test"))
                Plugin.Test();
        }
    }
}


#endif
