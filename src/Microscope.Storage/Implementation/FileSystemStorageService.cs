using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Minio.DataModel;

namespace Microscope.Storage
{
    public class FileSystemStorageService : IStorageService
    {
        public FileSystemStorageService()
        {
            
        }

        public Task DeleteBlobAsync(string containerName, string blobName)
        {
            throw new NotImplementedException();
        }

        public Task<Stream> GetBlobAsync(string bucketName, string objectName)
        {
            throw new NotImplementedException();
        }

        public Task SaveBlobAsync(string containerName, string blobName, Stream data)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<string>> ListContainersAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<string>> ListBlobsAsync(string containerName)
        {
            throw new NotImplementedException();
        }

        public Task CreateContainerAsync(string containerName)
        {
            throw new NotImplementedException();
        }
    }
}
