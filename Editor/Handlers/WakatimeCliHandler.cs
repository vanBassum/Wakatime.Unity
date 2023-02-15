#if (UNITY_EDITOR)

namespace Wakatime
{

    public class WakatimeCliHandler : IWakatimeHandler
    {
        private Logger Logger { get; set; }
        private Settings Settings { get; set; }
        public WakatimeCliHandler(Logger logger, Settings settings)
        {
            Logger= logger;
            Settings= settings;
        }

        public void HandleHeartBeat(Heartbeat heartbeat)
        {
            throw new System.NotImplementedException();
        }

        public void Dispose()
        {
        }
    }
}


#endif
