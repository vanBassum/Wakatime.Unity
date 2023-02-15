#if (UNITY_EDITOR)


namespace Wakatime
{
    //https://github.com/wakatime/nuget-wakatime-shared-extension-utils/blob/0f376c8d6bbe9579ec72d47c122ad728c1965ae8/src/WakaTime.Shared.ExtensionUtils/Heartbeat.cs
    public class Heartbeat
    {
        public string Entity { get; set; }
        public string Timestamp { get; set; }
        public string Project { get; set; }
        public bool IsWrite { get; set; }
        public string BranchName { get; set; }
        public HeartbeatCategories? Category { get; set; }
        public EntityTypes? EntityType { get; set; }

        /// <summary>
        /// It's a workaround for serialization.
        /// More details https://bit.ly/3mJB1mP
        /// </summary>
        public override string ToString()
        {
            return $"{{\"entity\":\"{Entity.Replace("\\", "\\\\").Replace("\"", "\\\"")}\"," +
                $"\"timestamp\":{Timestamp}," +
                $"\"project\":\"{Project.Replace("\"", "\\\"")}\"," +
                $"\"is_write\":{IsWrite.ToString().ToLower()}," +
                $"\"category\":\"{Category.GetDescription()}\"," +
                $"\"entity_type\":\"{EntityType.GetDescription()}\"}}";
        }
    }
}


#endif
