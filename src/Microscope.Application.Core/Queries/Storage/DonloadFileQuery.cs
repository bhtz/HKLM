using System;
using System.IO;
using MediatR;

namespace Microscope.Application.Features.Queries.Storage
{
    public class DonloadFileQuery : IRequest<BlobQueryResult>
    {
        public string ContainerName { get; set; }
        public string Name { get; set; }
    }

    public class BlobQueryResult 
    {
        public string Name { get; set; }
        public Stream Data { get; set; }
    }
}
