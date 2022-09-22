#if (UNITY_EDITOR)
using System;
using UnityEditor.SceneManagement;
using UnityEngine;
using static log4net.Appender.RollingFileAppender;

namespace WakaTime
{
    [Serializable]
    public class Heartbeat
    {
        #region Documented on https://wakatime.com/developers
        /// <summary>entity heartbeat is logging time against, such as an absolute file path or domain</summary>
        public string entity;
        /// <summary>type of entity; can be file, app, or domain</summary>
        public string type;
        /// <summary>UNIX epoch timestamp; numbers after decimal point are fractions of a second</summary>
        public float time;
        /// <summary>category for this activity (optional); normally this is inferred automatically from type; can be coding, building, indexing, debugging, browsing, running tests, writing tests, manual testing, writing docs, code reviewing, researching, learning, or designing</summary>
        //public string category;
        /// <summary>project name (optional)</summary>
        public string project;
        /// <summary>branch name (optional)</summary>
        public string branch;
        /// <summary>language name (optional)</summary>
        public string language;
        /// <summary>comma separated list of dependencies detected from entity file (optional)</summary>
        //public string dependencies;
        /// <summary>total number of lines in the entity (when entity type is file)</summary>
        //public int lines;
        /// <summary>current line row number of cursor with the first line starting at 1 (optional)</summary>
        //public int lineno;
        /// <summary>current cursor column position starting from 1 (optional)</summary>
        //public int cursorpos;
        /// <summary>whether this heartbeat was triggered from writing to a file (optional)</summary>
        public bool is_write;
        #endregion


        public Heartbeat(string entity, string type)
        {
            this.entity = entity;
            this.type = type;
            this.time = DateTime.Now.ToUnixTimeFloat();
        }
    }
}


#endif
