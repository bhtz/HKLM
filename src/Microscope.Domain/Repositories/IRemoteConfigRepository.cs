using Microscope.Domain.Entities;
using Microscope.Domain.Kernel;

namespace Microscope.Domain.Aggregates.RemoteConfigAggregate
{
    public interface IRemoteConfigRepository : IRepository<RemoteConfig>
    {
    }
}
