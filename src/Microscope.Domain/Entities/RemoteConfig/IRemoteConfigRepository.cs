using System;
using Microscope.Domain.Entities;
using Microscope.Domain.Interfaces;

namespace Microscope.Domain.Aggregates.RemoteConfigAggregate
{
    public interface IRemoteConfigRepository : IRepository<RemoteConfig>
    {
        IUnitOfWork UnitOfWork { get; }
    }
}
