using System.Collections.Generic;
using MediatR;

namespace Microscope.Application.Features.Storage.Queries
{
    public class GetBlobsByContainerQuery : IRequest<IEnumerable<GetBlobsByContainerQueryResult>>
    {
        public string ContainerName { get; set; }
    }

    public class GetBlobsByContainerQueryResult
    {
        public string Name { get; set; }
    }
}
