#if (UNITY_EDITOR)
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace Wakatime
{
    public interface IWakatimeClient
    {
        public Task<Response<HeartbeatResponse>> SendHeartbeat(Heartbeat heartbeat, CancellationToken cancellationToken = default);
        public Task<Response<HeartbeatResponse>> SendHeartbeats(List<Heartbeat> heartbeats, CancellationToken cancellationToken = default);
    }
}


#endif
