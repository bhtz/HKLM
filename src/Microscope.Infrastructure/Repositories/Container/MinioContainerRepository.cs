using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microscope.Domain.Repositories;
using Microscope.Domain.SharedKernel;

namespace Microscope.Infrastructure.Repositories.Container
{
    public class MinioContainerRepository : IContainerRepository
    {
        public IUnitOfWork UnitOfWork => throw new NotImplementedException();

        public Task<List<Domain.Entities.Blob>> GetBlobsAsync(Domain.Entities.Container container)
        {
            throw new NotImplementedException();
        }
    }
}
