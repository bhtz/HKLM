using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Microscope.Storage
{
    public interface IStorageService
    {
        public Task SaveBlobAsync(string containerName, string blobName, Stream data);
        public Task DeleteBlobAsync(string containerName, string blobName);
        public Task<Stream> GetBlobAsync(string containerName, string blobName);
        public Task<IEnumerable<string>> ListBlobsAsync(string containerName);
        public Task<IEnumerable<string>> ListContainersAsync();
        public Task CreateContainerAsync(string containerName);
    }
}
