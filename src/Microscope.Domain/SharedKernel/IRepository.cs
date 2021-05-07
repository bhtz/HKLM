using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microscope.Domain.SharedKernel
{
    public interface IRepository<T> : IRepository<T, Guid> where T : class, IAggregateRoot<Guid>
    {

    }

    public interface IRepository<T, TId> where T : IAggregateRoot<TId>
    {
        IUnitOfWork UnitOfWork { get; }

        Task<T> GetByIdAsync(TId id);

        Task<List<T>> GetAllAsync();

        Task<T> AddAsync(T entity);

        Task UpdateAsync(T entity);

        Task DeleteAsync(T entity);
    }
}
