#if (UNITY_EDITOR)
using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace WakaTime
{
    public class Logger : IDisposable
    {
        public string Title { get; set; }
        public Levels LogLevel { get; set; }

        public Logger(string title, Levels logLevel)
        {
            Title = title;
            LogLevel = logLevel;
        }

        public void Log(Levels level, string message, [CallerMemberName] string memberName = "")
        {
            if (level <= LogLevel)
            {
                switch (level)
                { 
                    case Levels.Emergency:
                    case Levels.Alert:
                    case Levels.Critical:
                    case Levels.Error:
                        Debug.LogError($"[{Title}.{memberName}] {message}");
                        break;
                    case Levels.Warning:
                        Debug.LogWarning($"[{Title}.{memberName}] {message}");
                        break;
                    case Levels.Notice:
                    case Levels.Informational:
                    case Levels.Debug:
                        Debug.Log($"[{Title}.{memberName}] {message}");
                        break;
                }
            }
        }

        public void Dispose()
        {

        }

        public enum Levels
        {
            Emergency = 0,
            Alert = 1,
            Critical = 2,
            Error = 3,
            Warning = 4,
            Notice = 5,
            Informational = 6,
            Debug = 7,
        }
    }
}


#endif
