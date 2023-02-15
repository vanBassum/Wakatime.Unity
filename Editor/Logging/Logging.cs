#if (UNITY_EDITOR)
using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Wakatime
{
    public class Logger : IDisposable
    {
        public string Title { get; set; }
        public LogLevels LogLevel { get; set; }

        public Logger(string title, LogLevels logLevel)
        {
            Title = title;
            LogLevel = logLevel;
        }

        public void Log(LogLevels level, string message, [CallerMemberName] string memberName = "")
        {
            if (level <= LogLevel)
            {
                switch (level)
                { 
                    case LogLevels.Emergency:
                    case LogLevels.Alert:
                    case LogLevels.Critical:
                    case LogLevels.Error:
                        Debug.LogError($"[{Title}.{memberName}] {message}");
                        break;
                    case LogLevels.Warning:
                        Debug.LogWarning($"[{Title}.{memberName}] {message}");
                        break;
                    case LogLevels.Notice:
                    case LogLevels.Informational:
                    case LogLevels.Debug:
                        Debug.Log($"[{Title}.{memberName}] {message}");
                        break;
                }
            }
        }

        public void Dispose()
        {

        }

    }
}


#endif
