using System;
using Minio;
using Minio.DataModel;
using Minio.Exceptions;
using Microsoft.Extensions.Options;
using System.IO;
using System.Threading.Tasks;

namespace Microscope.Storage
{
    public class MinioStorageService : IStorageService
    {
        private readonly MinioClient _client;
        private readonly StorageOptions _options;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        public MinioStorageService(IOptions<StorageOptions> options)
        {
            _options = options.Value;
            this._client = new MinioClient(_options.Host, _options.Key, _options.Secret);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="prefix"></param>
        /// <param name="recursive"></param>
        /// <returns></returns>
        public async void GetObjectAsync(string bucketName, string objectName)
        {
            try
            {
                Stream s = new MemoryStream();
                await _client.GetObjectAsync(bucketName, objectName, 
                (stream) => 
                {
                    stream.CopyTo(s);
                });
            }
            catch (Exception e)
            {
                Console.WriteLine($"[Bucket]  Exception: {e}");
            }
        }
    }
}
