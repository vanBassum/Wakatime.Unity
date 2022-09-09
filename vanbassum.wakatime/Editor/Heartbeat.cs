#if (UNITY_EDITOR)
using System;
using UnityEngine;

namespace WakaTime
{
    [Serializable]
    public struct Heartbeat
    {
        public string entity;
        public string type;
        public float time;
        public string project;
        public string branch;
        public string plugin;
        public string language;
        public bool is_write;
        public bool is_debugging;


        public static Heartbeat Create(string projectName, bool debugging, string file, bool save = false)
        {
            Heartbeat heartbeat = new Heartbeat();
            heartbeat.entity = file == string.Empty ? "Unsaved Scene" : file;
            heartbeat.type = "file";
            heartbeat.time = (float)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
            heartbeat.project = projectName;
            heartbeat.branch = "unity-wakatime";
            heartbeat.plugin = "master";
            heartbeat.language = "unity";
            heartbeat.is_write = save;
            heartbeat.is_debugging = debugging;

            return heartbeat;
        }
    }

}


#endif
