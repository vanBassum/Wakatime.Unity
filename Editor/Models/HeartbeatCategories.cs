#if (UNITY_EDITOR)

using System.ComponentModel;

namespace Wakatime
{
    public enum HeartbeatCategories
    {
        [Description("coding")]
        Coding,
        [Description("building")]
        Building,
        [Description("indexing")]
        Indexing,
        [Description("debugging")]
        Debugging,
        [Description("running tests")]
        RunningTests,
        [Description("writing tests")]
        WritingTests,
        [Description("manual testing")]
        ManualTesting,
        [Description("code reviewing")]
        CodeReviewing,
        [Description("browsing")]
        Browsing,
        [Description("designing")]
        Designing
    }
}


#endif
