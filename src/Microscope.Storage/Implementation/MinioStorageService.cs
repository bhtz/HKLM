using System;
using Minio;
using Minio.DataModel;
using Minio.Exceptions;
using Microsoft.Extensions.Options;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Linq;

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
        /// <param name="containerName"></param>
        /// <param name="prefix"></param>
        /// <param name="recursive"></param>
        /// <returns></returns>
        public async Task<Stream> GetBlobAsync(string containerName, string blobName)
        {
            try
            {
                Stream fileStream = new MemoryStream();
                await this._client.GetObjectAsync(containerName, blobName, (stream) =>
                {
                    stream.CopyTo(fileStream);
                });

                return fileStream;
            }
            catch (Exception e)
            {
                throw new Exception($"[Bucket]  Exception: {e}");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="containerName"></param>
        /// <returns></returns>
        public async Task<IEnumerable<string>> ListBlobsAsync(string containerName)
        {
            List<string> blobs = new List<string>();

            try
            {
                IObservable<Item> observable = this._client.ListObjectsAsync(containerName, null, true);
                IDisposable subscription = observable.Subscribe(
                    item => blobs.Add(item.Key),
                    ex => throw ex);

                await observable.LastOrDefaultAsync();
                subscription.Dispose();
            }
            catch (MinioException e)
            {
                throw e;
            }

            Console.WriteLine(blobs.Count);
            return blobs;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="containerName"></param>
        /// <param name="blobName"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task SaveBlobAsync(string containerName, string blobName, Stream data)
        {
            await this._client.PutObjectAsync(containerName, blobName, data, data.Length);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="containerName"></param>
        /// <param name="blobName"></param>
        /// <returns></returns>
        public async Task DeleteBlobAsync(string containerName, string blobName)
        {
            await this._client.RemoveObjectAsync(containerName, blobName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="containerName"></param>
        /// <param name="blobName"></param>
        /// <returns></returns>
        private async Task<bool> BlobExistsAsync(string containerName, string blobName)
        {
            if (await this._client.BucketExistsAsync(containerName))
            {
                try
                {
                    await this._client.StatObjectAsync(containerName, blobName);
                }
                catch (Exception e)
                {
                    if (e is ObjectNotFoundException)
                    {
                        return false;
                    }

                    throw;
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// List all containers
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<string>> ListContainersAsync()
        {
            var result = await this._client.ListBucketsAsync();
            return result.Buckets.Select(x => x.Name);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="containerName"></param>
        /// <returns></returns>
        public async Task CreateContainerAsync(string containerName)
        {
            await this._client.MakeBucketAsync(containerName);
        }
    }
}
