﻿#if (UNITY_EDITOR)
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace Wakatime
{
    //https://github.com/wakatime/nuget-wakatime-shared-extension-utils/blob/0f376c8d6bbe9579ec72d47c122ad728c1965ae8/src/WakaTime.Shared.ExtensionUtils/JSONSerializer.cs
    public static class JSONSerializer
    {
        /// <summary>
        /// Serializes an object to JSON
        /// </summary>
        public static string Serialize<TType>(TType instance) where TType : class
        {
            var serializer = new DataContractJsonSerializer(typeof(TType));
            using (var stream = new MemoryStream())
            {
                serializer.WriteObject(stream, instance);
                return Encoding.Default.GetString(stream.ToArray());
            }
        }

        /// <summary>
        /// Serializes a collection of Heartbeat to JSON
        /// </summary>
        public static string SerializeArrayHeartbeat(List<Heartbeat> instance)
        {
            var heartbeats = string.Join(",", instance);
            return $"[{heartbeats}]";
        }

        /// <summary>
        /// DeSerializes an object from JSON
        /// </summary>
        public static TType DeSerialize<TType>(string json) where TType : class
        {
            using (var stream = new MemoryStream(Encoding.Default.GetBytes(json)))
            {
                var serializer = new DataContractJsonSerializer(typeof(TType));
                return serializer.ReadObject(stream) as TType;
            }
        }
    }
}


#endif
