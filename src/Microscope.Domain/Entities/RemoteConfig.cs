using System;

namespace Microscope.Domain.Entities
{
    public class RemoteConfig
    {
        public Guid Id { get; set; }
        public string Key { get; set; }
        public string Dimension { get; set; }
    }
}
