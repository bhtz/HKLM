using System;
using Minio.DataModel;

namespace Microscope.Storage
{
    public class FileSystemStorageService : IStorageService
    {
        public FileSystemStorageService()
        {
            
        }

        public void GetObjectAsync(string bucketName, string objectName)
        {
            throw new NotImplementedException();
        }
    }
}
