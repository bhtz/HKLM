using System.IO;
using MediatR;

namespace Microscope.Application.Features.Storage.Queries
{
    public class DownloadBlobQuery : IRequest<BlobDataQueryResult>
    {
        public string ContainerName { get; set; }
        public string BlobName { get; set; }
    }

    public class BlobDataQueryResult 
    {
        public string Name { get; set; }
        public Stream Data { get; set; }
    }
}
