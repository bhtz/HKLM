using System.Linq;
using Microscope.Domain.Entities;
using Microscope.Domain.SharedKernel;

namespace Microscope.Domain.Aggregates.RemoteConfigAggregate
{
    public interface IRemoteConfigRepository : IRepository<RemoteConfig>
    {
        
    }
}
