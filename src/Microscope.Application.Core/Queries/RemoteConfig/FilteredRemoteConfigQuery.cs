using System;
using System.Collections.Generic;
using MediatR;

namespace Microscope.Application.Core.Queries.RemoteConfig
{
    public class FilteredRemoteConfigQuery : IRequest<IEnumerable<RemoteConfigQueryResult>>
    {
        
    }

    public class RemoteConfigQueryResult
    {
        public Guid Id { get; set; }
        public string Key { get; set; }
        public string Dimension { get; set; }
    }
}
