#if (UNITY_EDITOR)
using System;
using System.Linq;
using System.Reflection;
using UnityEditor;

namespace Wakatime
{
    public class SettingsManager : IDisposable
    {
        private Logger Logger { get; set; }
        public SettingsManager(Logger logger)
        {
            Logger= logger;
        }


        SettingHandler[] handlers = new SettingHandler[] {
            new SettingHandler(typeof(string), LoadString, SaveString),
            new SettingHandler(typeof(int), LoadInt, SaveInt),
        };



        public Settings LoadSettings()
        {
            Settings result = new Settings();
            var properties = result.GetType().GetProperties();
            foreach (var property in properties)
            {
                if (property.GetCustomAttribute<SettingAttribute>() is SettingAttribute attribute)
                {
                    SettingHandler handler = handlers.FirstOrDefault(h => h.Type == property.PropertyType);
                    if (handler != null)
                    {
                        handler.Load(property, result, attribute.Key);
                    }
                    else
                    {
                        Logger.Log(LogLevels.Error, $"Couln't load setting {property.Name}, using default.");
                    }
                }
            }
            return result;
        }

        public void SaveSettings(Settings settings)
        {
            var properties = settings.GetType().GetProperties();
            foreach (var property in properties)
            {
                if (property.GetCustomAttribute<SettingAttribute>() is SettingAttribute attribute)
                {
                    SettingHandler handler = handlers.FirstOrDefault(h => h.Type == property.PropertyType);
                    if (handler != null)
                    {
                        handler.Save(property, settings, attribute.Key);
                    }
                    else
                    {
                        Logger.Log(LogLevels.Error, $"Couln't save setting {property.Name}.");
                    }
                }
            }
        }





        static void LoadString(PropertyInfo property, object obj, string key) => property.SetValue(obj, EditorPrefs.GetString(key));
        static void SaveString(PropertyInfo property, object obj, string key) => EditorPrefs.SetString(key, (string)property.GetValue(key));
        static void LoadInt(PropertyInfo property, object obj, string key) => property.SetValue(obj, EditorPrefs.GetString(key));
        static void SaveInt(PropertyInfo property, object obj, string key) => EditorPrefs.SetInt(key, (int)property.GetValue(key));

        public void Dispose()
        {
        }
    }




}


#endif
