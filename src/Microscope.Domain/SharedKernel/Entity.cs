using System;
using System.Collections.Generic;

namespace Microscope.Domain.SharedKernel
{
    public class Entity
    {
        public List<DomainEvent> _domainEvents { get; private set; }
        public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents?.AsReadOnly();

        public void AddDomainEvent(DomainEvent eventItem)
        {
            _domainEvents = _domainEvents ?? new List<DomainEvent>();
            _domainEvents.Add(eventItem);
        }

        public void RemoveDomainEvent(DomainEvent eventItem)
        {
            _domainEvents?.Remove(eventItem);
        }

        public void ClearDomainEvents()
        {
            _domainEvents?.Clear();
        }
    }
}
