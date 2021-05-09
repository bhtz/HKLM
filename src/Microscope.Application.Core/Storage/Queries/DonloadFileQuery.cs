using System.IO;
using MediatR;

namespace Microscope.Application.Features.Storage.Queries
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
