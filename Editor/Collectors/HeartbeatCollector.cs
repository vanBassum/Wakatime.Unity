#if (UNITY_EDITOR)

using System;
using System.Globalization;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Wakatime
{


    /// <summary>
    /// Catches the events and creates heartbeats
    /// </summary>
    public class HeartbeatCollector : IHeartbeatCollector
    {
        public event EventHandler<Heartbeat> OnHeartbeat;
        private Logger Logger { get; }
        private IGitClient GitClient { get; }
        private Settings Settings { get; set; }
        public HeartbeatCollector(Logger logger, Settings settings)
        {
            Logger = logger;
            Settings = settings;
            switch (Settings.GitOptions)
            {
                case GitClientTypes.GitCLI:
                    GitClient = new GitCliClient(Logger);
                    break;
                case GitClientTypes.FileIO:
                    GitClient = new GitFileIOClient(Logger);
                    break;
            }

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

        private void ThrowHeartbeat(Heartbeat heartbeat)
        {
            OnHeartbeat?.Invoke(this, heartbeat);
        }

        private string GetEntity(UnityEngine.SceneManagement.Scene? scene = null)
        {
            scene ??= EditorSceneManager.GetActiveScene();
            var currentScene = scene?.path;
            string entity = "Unsaved Scene";
            if (!string.IsNullOrEmpty(currentScene))
                entity = Application.dataPath + "/" + currentScene.Substring("Assets/".Length);
            return entity;
        }

        private void EditorApplication_contextualPropertyMenu(GenericMenu menu, SerializedProperty property)
        {
            var entity = GetEntity();
            var heartbeat = CreateHeartbeat(entity);
            ThrowHeartbeat(heartbeat);
        }

        private void EditorSceneManager_newSceneCreated(UnityEngine.SceneManagement.Scene scene, NewSceneSetup setup, NewSceneMode mode)
        {
            var entity = GetEntity(scene);
            var heartbeat = CreateHeartbeat(entity);
            ThrowHeartbeat(heartbeat);
        }

        private void EditorSceneManager_sceneClosing(UnityEngine.SceneManagement.Scene scene, bool removingScene)
        {
            var entity = GetEntity(scene);
            var heartbeat = CreateHeartbeat(entity);
            ThrowHeartbeat(heartbeat);
        }

        private void EditorSceneManager_sceneOpened(UnityEngine.SceneManagement.Scene scene, OpenSceneMode mode)
        {
            var entity = GetEntity(scene);
            var heartbeat = CreateHeartbeat(entity);
            ThrowHeartbeat(heartbeat);
        }

        private void EditorSceneManager_sceneSaved(UnityEngine.SceneManagement.Scene scene)
        {
            var entity = GetEntity(scene);
            var heartbeat = CreateHeartbeat(entity);
			heartbeat.IsWrite = true;
            ThrowHeartbeat(heartbeat);
        }

        private void EditorApplication_hierarchyChanged()
        {
            var entity = GetEntity();
            var heartbeat = CreateHeartbeat(entity);
            ThrowHeartbeat(heartbeat);
        }

        private void EditorApplication_playModeStateChanged(PlayModeStateChange obj)
        {
            var entity = GetEntity();
            var heartbeat = CreateHeartbeat(entity);
            if (obj == PlayModeStateChange.EnteredPlayMode)
                heartbeat.Category = HeartbeatCategories.Debugging;
            ThrowHeartbeat(heartbeat);
        }

        private Heartbeat CreateHeartbeat(string entity)
        {
            string workingDir = Path.GetDirectoryName(entity);
            Heartbeat heartbeat = new Heartbeat
            {
                Entity = entity,
                EntityType = EntityTypes.File,
                Timestamp = DateTime.Now.ToUnixTimeFloat().ToString(CultureInfo.InvariantCulture),
                Project = Settings.ProjectName,
                BranchName = GitClient?.GetBranchName(workingDir),
				IsWrite = false,
                Category = Application.isPlaying ? HeartbeatCategories.Debugging : HeartbeatCategories.Coding,
            };
            return heartbeat;
        }
    }
}


#endif
