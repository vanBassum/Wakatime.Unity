#if (UNITY_EDITOR)
using System;
using System.Reflection;

namespace Wakatime
{
    class SettingHandler
    {
        public SettingHandler(Type type, Action<PropertyInfo, object, string> load, Action<PropertyInfo, object, string> save)
        {
            Type = type;
            Load = load;
            Save = save;
        }

        public Type Type { get; set; }
        public Action<PropertyInfo, object, string> Load { get; set; }
        public Action<PropertyInfo, object, string> Save { get; set; }
    }




}


#endif
