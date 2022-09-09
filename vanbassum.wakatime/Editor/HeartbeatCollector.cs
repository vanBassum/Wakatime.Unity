#if (UNITY_EDITOR)

using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace WakaTime
{
    /// <summary>
    /// Catches the events and creates heartbeats
    /// </summary>
    public class HeartbeatCollector : IDisposable
    {
        private string ProjectName { get; }
        public event EventHandler<Heartbeat> OnHeartbeat;
        private Logger Logger { get; }
        public HeartbeatCollector(Logger logger, string projectName)
        {
            Logger = logger;
            ProjectName = projectName;
            EditorApplication.playModeStateChanged += EditorApplication_playModeStateChanged;
            EditorApplication.contextualPropertyMenu += EditorApplication_contextualPropertyMenu;
            EditorApplication.hierarchyChanged += EditorApplication_hierarchyChanged;
            EditorSceneManager.sceneSaved += EditorSceneManager_sceneSaved;
            EditorSceneManager.sceneOpened += EditorSceneManager_sceneOpened;
            EditorSceneManager.sceneClosing += EditorSceneManager_sceneClosing;
            EditorSceneManager.newSceneCreated += EditorSceneManager_newSceneCreated;
        }

        public void Dispose()
        {
            EditorApplication.playModeStateChanged -= EditorApplication_playModeStateChanged;
            EditorApplication.contextualPropertyMenu -= EditorApplication_contextualPropertyMenu;
            EditorApplication.hierarchyChanged -= EditorApplication_hierarchyChanged;
            EditorSceneManager.sceneSaved -= EditorSceneManager_sceneSaved;
            EditorSceneManager.sceneOpened -= EditorSceneManager_sceneOpened;
            EditorSceneManager.sceneClosing -= EditorSceneManager_sceneClosing;
            EditorSceneManager.newSceneCreated -= EditorSceneManager_newSceneCreated;
        }

        private void EditorApplication_contextualPropertyMenu(GenericMenu menu, SerializedProperty property)
        {
            Logger.Log(Logger.Levels.Debug, "Created heartbeat");
            var heartbeat = CreateHeartbeat();
            OnHeartbeat?.Invoke(this, heartbeat);
        }

        private void EditorSceneManager_newSceneCreated(UnityEngine.SceneManagement.Scene scene, NewSceneSetup setup, NewSceneMode mode)
        {
            Logger.Log(Logger.Levels.Debug, "Created heartbeat");
            var heartbeat = CreateHeartbeat();
            OnHeartbeat?.Invoke(this, heartbeat);
        }

        private void EditorSceneManager_sceneClosing(UnityEngine.SceneManagement.Scene scene, bool removingScene)
        {
            Logger.Log(Logger.Levels.Debug, "Created heartbeat");
            var heartbeat = CreateHeartbeat();
            OnHeartbeat?.Invoke(this, heartbeat);
        }

        private void EditorSceneManager_sceneOpened(UnityEngine.SceneManagement.Scene scene, OpenSceneMode mode)
        {
            Logger.Log(Logger.Levels.Debug, "Created heartbeat");
            var heartbeat = CreateHeartbeat();
            OnHeartbeat?.Invoke(this, heartbeat);
        }

        private void EditorSceneManager_sceneSaved(UnityEngine.SceneManagement.Scene scene)
        {
            Logger.Log(Logger.Levels.Debug, "Created heartbeat");
            var heartbeat = CreateHeartbeat();
            heartbeat.is_write = true;
            OnHeartbeat?.Invoke(this, heartbeat);
        }

        private void EditorApplication_hierarchyChanged()
        {
            Logger.Log(Logger.Levels.Debug, "Created heartbeat");
            var heartbeat = CreateHeartbeat();
            OnHeartbeat?.Invoke(this, heartbeat);
        }

        private void EditorApplication_playModeStateChanged(PlayModeStateChange obj)
        {
            Logger.Log(Logger.Levels.Debug, "Created heartbeat");
            var heartbeat = CreateHeartbeat();
            OnHeartbeat?.Invoke(this, heartbeat);
        }

        private Heartbeat CreateHeartbeat()
        {
            var currentScene = EditorSceneManager.GetActiveScene().path;
            string entity = "Unsaved Scene";
            if (!string.IsNullOrEmpty(currentScene))
                entity = Application.dataPath + "/" + currentScene.Substring("Assets/".Length);
            string type = "file";

            Heartbeat heartbeat = new Heartbeat(entity, type);
            heartbeat.project = ProjectName;
            heartbeat.language = "unity";
            return heartbeat;
        }

        
    }
}


#endif
