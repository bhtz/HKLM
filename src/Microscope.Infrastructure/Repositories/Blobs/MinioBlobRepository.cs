using System;
using System.IO;
using System.Threading.Tasks;
using Microscope.Domain.Entities;
using Microscope.Domain.Repositories;
using Microscope.Domain.SharedKernel;
using Microsoft.Extensions.Options;
using Minio;

namespace Microscope.Infrastructure.Repositories.Blobs
{
    public class MinioBlobRepository : IBlobRepository
    {
        private readonly MicroscopeDbContext _context;
        private readonly MinioClient _client;
        private readonly StorageOptions _options;
        
        public IUnitOfWork UnitOfWork
        {
            get { return _context; }
        }

        public MinioBlobRepository(MicroscopeDbContext context, IOptions<StorageOptions> options)
        {
            _options = options.Value;
            _context = context;
            _client = new MinioClient(_options.Host, _options.Key, _options.Secret);
        }

        public async Task DeleteAsync(Blob blob)
        {
            await this._client.RemoveObjectAsync(blob.ContainerName, blob.Name);
        }

        public async Task<Stream> GetBlobData(Blob blob)
        {
            try
            {
                Stream ms = new MemoryStream();

                await this._client.GetObjectAsync(blob.ContainerName, blob.Name, (stream) =>
                {
                    stream.CopyTo(ms);
                });

                return ms;
            }
            catch (Exception e)
            {
                throw new Exception($"[Bucket]  Exception: {e}");
            }
        }

        public async Task SaveAsync(Blob blob)
        {
            await this._client
                .PutObjectAsync(blob.ContainerName, blob.Name, blob.ToStream(), blob.GetSize(), "application/octet-stream");
        }
    }
}
