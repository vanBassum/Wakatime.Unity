#if (UNITY_EDITOR)
using System;

namespace Wakatime
{
    public class SettingAttribute : Attribute
    {
        public string Key;

        public SettingAttribute(string key)
        {
            Key = key;
        }
    }




}


#endif
