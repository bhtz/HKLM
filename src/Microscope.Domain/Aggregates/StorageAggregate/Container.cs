using System;
using System.Collections.Generic;

namespace Microscope.Domain.Aggregates.StorageAggregate
{
    public class Container
    {
        #region ctor

        protected Container()
        {
            this._blobs = new List<Blob>();
        }

        protected Container(Guid id, string name): this()
        {
            this.Id = id;
            this.Name = name;
        }

        public static Container NewContainer(Guid id, string name)
        {
            return new Container(id, name);
        }

        #endregion

        #region properties

        public Guid Id { get; private set; }
        public string Name { get; private set; }

        private readonly List<Blob> _blobs;
        public IReadOnlyCollection<Blob> Blobs => _blobs;

        #endregion

        internal void AddBlob(Blob item)
        {
            _blobs.Add(item);
        }
    }
}
