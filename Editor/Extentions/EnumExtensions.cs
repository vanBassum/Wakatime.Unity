#if (UNITY_EDITOR)

using System;
using System.ComponentModel;
using System.Linq;

namespace Wakatime
{
    //https://github.com/wakatime/nuget-wakatime-shared-extension-utils/blob/0f376c8d6bbe9579ec72d47c122ad728c1965ae8/src/WakaTime.Shared.ExtensionUtils/EnumExtensions.cs
    public static class EnumExtensions
    {
        public static string GetDescription(this Enum e)
        {
            if (e is null) return null;

            var attr = (DescriptionAttribute)e.GetType().GetField(e.ToString())
                .GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault();
            return attr?.Description ?? e.ToString();
        }
    }
}


#endif
