#if (UNITY_EDITOR)

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Timers;
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

        Dictionary<string, DateTime> heartbeatHistory = new Dictionary<string, DateTime>();
        

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

        private bool CheckEntityTimeout(string entity)
        {
            if (heartbeatHistory.TryGetValue(entity, out DateTime value))
            {
                if ((DateTime.Now - value) < Settings.SameFileTimeout)
                    return false;
            }
            heartbeatHistory[entity] = DateTime.Now;
            return true;
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
            if(CheckEntityTimeout(entity))
            {
                var heartbeat = CreateHeartbeat(entity);
                ThrowHeartbeat(heartbeat);
            }
        }

        private void EditorSceneManager_newSceneCreated(UnityEngine.SceneManagement.Scene scene, NewSceneSetup setup, NewSceneMode mode)
        {
            var entity = GetEntity(scene);
            if (CheckEntityTimeout(entity))
            {
                var heartbeat = CreateHeartbeat(entity);
                ThrowHeartbeat(heartbeat);
            }
        }

        private void EditorSceneManager_sceneClosing(UnityEngine.SceneManagement.Scene scene, bool removingScene)
        {
            var entity = GetEntity(scene);
            if (CheckEntityTimeout(entity))
            {
                var heartbeat = CreateHeartbeat(entity);
                ThrowHeartbeat(heartbeat);
            }
        }

        private void EditorSceneManager_sceneOpened(UnityEngine.SceneManagement.Scene scene, OpenSceneMode mode)
        {
            var entity = GetEntity(scene);
            if (CheckEntityTimeout(entity))
            {
                var heartbeat = CreateHeartbeat(entity);
                ThrowHeartbeat(heartbeat);
            }
        }

        private void EditorSceneManager_sceneSaved(UnityEngine.SceneManagement.Scene scene)
        {
            var entity = GetEntity(scene);
            if (CheckEntityTimeout(entity))
            {
                var heartbeat = CreateHeartbeat(entity);
                heartbeat.is_write = true;
                ThrowHeartbeat(heartbeat);
            }
        }

        private void EditorApplication_hierarchyChanged()
        {
            var entity = GetEntity();
            if (CheckEntityTimeout(entity))
            {
                var heartbeat = CreateHeartbeat(entity);
                ThrowHeartbeat(heartbeat);
            }
        }

        private void EditorApplication_playModeStateChanged(PlayModeStateChange obj)
        {
            var entity = GetEntity();
            if (CheckEntityTimeout(entity))
            {
                var heartbeat = CreateHeartbeat(entity);
                ThrowHeartbeat(heartbeat);
            }
        }

        private Heartbeat CreateHeartbeat(string entity)
        {
            Heartbeat heartbeat = new Heartbeat(entity, "file");
            heartbeat.project = ProjectName;
            heartbeat.language = "Unity";
            heartbeat.branch = GetBranchName(Application.dataPath);
            return heartbeat;
        }

        private string GetBranchName(string workingDir)
        {
            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo("git"); //No .exe, I assume this work on linux and macos.

                startInfo.UseShellExecute = false;
                startInfo.WorkingDirectory = workingDir;
                startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                startInfo.RedirectStandardInput = true;
                startInfo.RedirectStandardOutput = true;
                startInfo.Arguments = "rev-parse --abbrev-ref HEAD";

                using Process process = new Process();
                process.StartInfo = startInfo;
                process.Start();

                string branchname = process.StandardOutput.ReadLine();
                return branchname;
            }
            catch(Exception ex)
            {
                //Todo, figure out if git exists on this machine.
                //Also, figure out if this is even a git repo.
                Logger.Log(Logger.Levels.Warning, "Couln't determine branchname, is git installed?");
            }
            return null;
        }
        
    }
}


#endif
