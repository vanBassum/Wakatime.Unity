#if (UNITY_EDITOR)

namespace Wakatime
{
    public class WakatimeNativeHandler : IWakatimeHandler
    {
        public void Dispose()
        {
            throw new System.NotImplementedException();
        }

        public void HandleHeartBeat(Heartbeat heartbeat)
        {
            throw new System.NotImplementedException();
        }
    }
}


#endif
