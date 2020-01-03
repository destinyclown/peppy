namespace Peppy.Core
{
    public interface IRegistryHost : IManageServiceInstances,
         IManageHealthChecks,
         IResolveServiceInstances
    {
    }
}