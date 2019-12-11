using System;
using System.Threading;
using System.Threading.Tasks;

namespace Peppy.Router
{
    public interface IPollingServiceSubscriber : IServiceSubscriber
    {
        Task StartSubscription(CancellationToken ct = default);

        event EventHandler EndpointsChanged;
    }
}
