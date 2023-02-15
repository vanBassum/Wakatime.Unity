#if (UNITY_EDITOR)
using System;
using UnityEngine;

namespace Wakatime
{
    public class Settings : IDisposable, ICloneable
    {
        [Setting("WakaTime/Enabled")]
        public bool Enabled { get; set; } = false;

        [Setting("WakaTime/APIKey")]
        public string ApiKey { get; set; } = "";

        [Setting("WakaTime/GitOptions")]
        public GitClientTypes GitOptions { get; set; }

        [Setting("WakaTime/WakatimeHandlerType")]
        public WakatimeHandlerTypes WakatimeHandlerType { get; set; }

        [Setting("WakaTime/WakatimeCliBinary")]
        public string WakatimeCliBinary { get; set; }

        [Setting("WakaTime/LogLevel")]
        public LogLevels LogLevel { get; set; }


        public string ProjectName => Application.productName;
        public string ApiUri => "https://api.wakatime.com/api/v1/";
        public TimeSpan SameFileTimeout => TimeSpan.FromMinutes(2);

        public void Dispose()
        {
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}


#endif
