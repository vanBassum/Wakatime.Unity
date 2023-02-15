#if (UNITY_EDITOR)

using System;

namespace Wakatime
{
    public interface IHeartbeatCollector : IDisposable
    {
        event EventHandler<Heartbeat> OnHeartbeat;
    }


}


#endif
