using System;
using System.IO;
using System.Threading.Tasks;
using Microscope.Domain.Repositories;
using Microscope.Domain.SharedKernel;

namespace Microscope.Infrastructure.Repositories.Blob
{
    public class MinioBlobRepository : IBlobRepository
    {
        public IUnitOfWork UnitOfWork => throw new NotImplementedException();

        public Task<Stream> GetBlobData(Domain.Entities.Blob blob)
        {
            throw new NotImplementedException();
        }
    }
}
