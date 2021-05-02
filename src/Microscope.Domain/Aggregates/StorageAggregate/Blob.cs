using System;
using System.IO;

namespace Microscope.Domain.Aggregates.StorageAggregate
{
    public class Blob
    {
        #region ctor 

        protected Blob(Guid id, string name)
        {
            this.Id = id;
            this.Name = name;
        }

        protected Blob(Guid id, string name, byte[] data) : this(id, name)
        {
            this.Data = data;
        }

        protected Blob(Guid id, string name, Stream data) : this(id, name)
        {
            data.Read(this.Data);
        }

        public static Blob NewBlob(Guid id, string name, Stream data)
        {
            return new Blob(id, name, data);
        }

        public static Blob NewBlob(Guid id, string name, byte[] data)
        {
            return new Blob(id, name, data);
        }

        #endregion

        public Guid Id { get; private set; }
        public byte[] Data { get; private set; }
        public string Name { get; private set; }
    }
}
