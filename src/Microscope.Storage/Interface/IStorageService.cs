using System;
using System.IO;
using Minio.DataModel;

namespace Microscope.Storage
{
    public interface IStorageService
    {
        public void GetObjectAsync(string bucketName, string objectName);
    }
}
