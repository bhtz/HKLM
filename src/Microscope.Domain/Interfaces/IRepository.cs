using System.Linq;

namespace Microscope.Domain.Interfaces
{
    public interface IRepository<T> where T : IAggregateRoot
    {
        // IUnitOfWork UnitOfWork { get; }

        // IQueryable<T> Entities { get; }

        // Task<T> GetByIdAsync(Guid id);

        // Task<List<T>> GetAllAsync();

        // Task<List<T>> GetPagedReponseAsync(int pageNumber, int pageSize);

        // Task<T> AddAsync(T entity);

        // Task UpdateAsync(T entity);

        // Task DeleteAsync(T entity);
    }
}
