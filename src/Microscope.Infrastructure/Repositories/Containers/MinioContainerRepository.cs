using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Microscope.Domain.Entities;
using Microscope.Domain.Repositories;
using Microscope.Domain.SharedKernel;
using Microsoft.Extensions.Options;
using Minio;
using Minio.Exceptions;

namespace Microscope.Infrastructure.Repositories.Containers
{
    public class MinioContainerRepository : IContainerRepository
    {
        private readonly MicroscopeDbContext _context;
        private readonly MinioClient _client;
        private readonly StorageOptions _options;
        
        public IUnitOfWork UnitOfWork
        {
            get { return _context; }
        }

        public MinioContainerRepository(MicroscopeDbContext context, IOptions<StorageOptions> options)
        {
            _options = options.Value;
            _context = context;
            _client = new MinioClient(_options.Host, _options.Key, _options.Secret);
        }

        public async Task AddAsync(Container entity)
        {
            await this._client.MakeBucketAsync(entity.Name);
        }

        public async Task DeleteAsync(Container entity)
        {
            await this._client.RemoveBucketAsync(entity.Name);
        }

        public async Task<List<Container>> GetAllAsync()
        {
            var result = await this._client.ListBucketsAsync();

            return result.Buckets
                .Select(x => Container.NewContainer(x.Name))
                .ToList();
        }

        public async Task<List<Blob>> GetBlobsAsync(Container container)
        {
            var result = await this._client.ListObjectsAsync(container.Name).ToList();
            return result
                .Select(x => Blob.NewBlob(x.Key, container.Name, new byte[0]))
                .ToList();
        }

        private async Task<bool> BlobExistsAsync(string containerName, string blobName)
        {
            return await this._client.BucketExistsAsync(containerName);
        }

        public Task UpdateAsync(Container entity)
        {
            throw new Exception("Update container not supported by Minio storage adapter");
        }
    }
}
