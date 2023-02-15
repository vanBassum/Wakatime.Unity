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
        public GitClientTypes GitOptions { get; set; } = GitClientTypes.Disabled;

        [Setting("WakaTime/WakatimeHandlerType")]
        public WakatimeHandlerTypes WakatimeHandlerType { get; set; } = WakatimeHandlerTypes.WakatimeCli;

        [Setting("WakaTime/WakatimeCliBinary")]
        public string WakatimeCliBinary { get; set; } = "wakatime.exe";

        [Setting("WakaTime/LogLevel")]
        public LogLevels LogLevel { get; set; } = LogLevels.Informational;




        public string ProjectName => Application.productName;
        public string ApiUri => "https://api.wakatime.com/api/v1/";
        public TimeSpan HeartbeatFrequency => TimeSpan.FromMinutes(2);


        //[Setting("WakaTime/AutoUpdateCli")]
        //public bool AutoUpdateCli { get; set; } = true;
        //public string S3UrlPrefix => "https://wakatime-cli.s3-us-west-2.amazonaws.com/";
        //public string GithubDownloadPrefix => "https://github.com/wakatime/wakatime-cli/releases/download";
        //public string GithubReleasesAlphaUrl => "https://api.github.com/repos/wakatime/wakatime-cli/releases?per_page=1";
        //public string GithubReleasesStableUrl =>"https://api.github.com/repos/wakatime/wakatime-cli/releases/latest";


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
