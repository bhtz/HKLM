using System.IO;
using Microscope.Domain.SharedKernel;

namespace Microscope.Domain.Entities
{
    public class Blob : IAggregateRoot
    {
        #region ctor

        protected Blob(string name, string containerName)
        {
            this.Id = name;
            this.ContainerName = containerName;
        }

        protected Blob(string name, string containerName, byte[] data) : this(name, containerName)
        {
            this.Data = data;
        }

        protected Blob(string name, string containerName, Stream data) : this(name, containerName)
        {
            data.Read(this.Data);
        }

        public static Blob NewBlob(string name, string containerName, Stream data)
        {
            return new Blob(name, containerName, data);
        }

        public static Blob NewBlob(string name, string containerName, byte[] data)
        {
            return new Blob(name, containerName, data);
        }

        #endregion

        public string Id { get; private set; }
        public byte[] Data { get; private set; }
        public string Name () => Id;
        public string ContainerName { get; private set; }
    }
}