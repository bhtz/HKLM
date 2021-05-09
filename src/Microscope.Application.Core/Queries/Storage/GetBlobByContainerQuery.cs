using System;
using System.Collections.Generic;
using MediatR;

namespace Microscope.Application.Features.Queries.Storage
{
    public class GetBlobByContainerQuery : IRequest<IEnumerable<GetBlobByContainerQueryResult>>
    {
        public string ContainerName { get; set; }
    }

    public class GetBlobByContainerQueryResult 
    {
        public string Name { get; set; }
    }
}
