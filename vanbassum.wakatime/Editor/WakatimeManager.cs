#if (UNITY_EDITOR)

namespace WakaTime
{
    public class WakatimeManager
    {
        private WakatimeApiClient Client { get; }
        private string ProjectName { get; }
        private bool Enabled { get; }

        public WakatimeManager(bool enabled, string projectName, string apiUri, string apiKey)
        {
            ProjectName = projectName;
            Enabled = enabled;
            Client = new WakatimeApiClient(apiUri, apiKey);
        }

        public void Stop()
        {

        }

        public async void Start()
        {
            if (!Enabled)
                return;

            if (string.IsNullOrWhiteSpace(ProjectName))
                return;

            var response = await Client.SendHearthbeat(Heartbeat.Create("Test", false, "test.cs"));


        }
    }

}


#endif
