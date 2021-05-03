using System;
using Microscope.Domain.Interfaces;

namespace Microscope.Domain.Aggregates.StorageAggregate
{
    public interface IStorageRepository : IRepository<Container>
    {
        
    }
}
