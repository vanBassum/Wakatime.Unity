#if (UNITY_EDITOR)

using System;

namespace Wakatime
{
    public interface IWakatimeHandler : IDisposable
    {
        void HandleHeartBeat(Heartbeat heartbeat);
    }

}


#endif
