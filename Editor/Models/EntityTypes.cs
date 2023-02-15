#if (UNITY_EDITOR)

using System.ComponentModel;

namespace Wakatime
{
    public enum EntityTypes
    {
        [Description("file")]
        File,
        [Description("domain")]
        Domain,
        [Description("app")]
        App
    }
}


#endif
