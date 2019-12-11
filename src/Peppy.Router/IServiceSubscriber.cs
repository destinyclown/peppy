using Peppy.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Peppy.Router
{
    public interface IServiceSubscriber : IDisposable
    {
        Task<List<RegistryInformation>> Endpoints(CancellationToken ct = default(CancellationToken));
    }
}
