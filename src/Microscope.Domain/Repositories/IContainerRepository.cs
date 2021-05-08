using System.Collections.Generic;
using System.Threading.Tasks;
using Microscope.Domain.Entities;
using Microscope.Domain.SharedKernel;

namespace Microscope.Domain.Repositories
{
    public interface IContainerRepository : IRepository<Container, string>
    {
        Task<List<Blob>> GetBlobsAsync(Container container);

        Task<List<Container>> GetAllAsync();

        Task AddAsync(Container entity);

        Task UpdateAsync(Container entity);

        Task DeleteAsync(Container entity);
    }
}
