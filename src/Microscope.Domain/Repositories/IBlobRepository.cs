using System.IO;
using System.Threading.Tasks;
using Microscope.Domain.Entities;
using Microscope.Domain.SharedKernel;

namespace Microscope.Domain.Repositories
{
    public interface IBlobRepository : IRepository<Blob>
    {
        Task<Stream> GetBlobData(Blob blob);
    }
}
