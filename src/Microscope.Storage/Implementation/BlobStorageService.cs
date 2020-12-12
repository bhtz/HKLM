using System;
using Minio.DataModel;

namespace Microscope.Storage
{
    public class BlobStorageService : IStorageService
    {
        public BlobStorageService()
        {
            
        }

        public void GetObjectAsync(string bucketName, string objectName)
        {
            throw new NotImplementedException();
        }
    }
}
