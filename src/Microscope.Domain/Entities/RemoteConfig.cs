using System;
using Microscope.Domain.SharedKernel;

namespace Microscope.Domain.Entities
{
    public class RemoteConfig : IAggregateRoot
    {
        #region ctor
        protected RemoteConfig()
        {

        }
        
        protected RemoteConfig(Guid id, string key, string dimension) : this()
        {
            this.Id = id;
            this.Key = key;
            this.Dimension = dimension;
        }

        public static RemoteConfig NewRemoteConfig(Guid id, string key, string dimension)
        {
            return new RemoteConfig(id, key, dimension);
        }

        #endregion

        public Guid Id { get; private set; }
        public string Key { get; private set; }
        public string Dimension { get; private set; }

        public void Update(string key, string dimension)
        {
            this.Key = key;
            this.Dimension = dimension;
        }
    }
}
