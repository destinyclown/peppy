using System;
using System.Collections.Generic;
using System.Text;

namespace Peppy.Core
{
    public interface IRegistryHost : IManageServiceInstances,
         IManageHealthChecks,
         IResolveServiceInstances
    {
    }
}
