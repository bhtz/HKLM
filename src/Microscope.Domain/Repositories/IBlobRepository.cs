using System.IO;
using System.Threading.Tasks;
using Microscope.Domain.Entities;
using Microscope.Domain.SharedKernel;

namespace Microscope.Domain.Repositories
{
    public interface IBlobRepository : IRepository<Blob, string>
    {
        Task<Stream> GetBlobData(Blob blob);

        Task SaveAsync(Blob blob);

        Task DeleteAsync(Blob blob);
    }
}
