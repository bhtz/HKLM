using System;
using Minio.DataModel;

namespace Microscope.Storage
{
    public class AwsStorageService : IStorageService
    {
        public AwsStorageService()
        {
            
        }

        public void GetObjectAsync(string bucketName, string objectName)
        {
            throw new NotImplementedException();
        }
    }
}
