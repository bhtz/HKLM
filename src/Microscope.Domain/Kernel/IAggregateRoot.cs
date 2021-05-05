using System;

namespace Microscope.Domain.Kernel
{
    public interface IAggregateRoot : IAggregateRoot<Guid>
    {
        
    }

    public interface IAggregateRoot<TId>
    {
        
    }
}
