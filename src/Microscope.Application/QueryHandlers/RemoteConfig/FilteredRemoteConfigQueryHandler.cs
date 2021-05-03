using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microscope.Application.Core.Queries.Analytic;
using Microscope.Application.Core.Queries.RemoteConfig;
using Microscope.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Microscope.Application.Features.Analytic.Queries
{
    public class FilteredRemoteConfigQueryHandler : IRequestHandler<FilteredRemoteConfigQuery, IEnumerable<RemoteConfigQueryResult>>
    {
        private readonly MicroscopeDbContext _microscopeDbContext;
        private readonly IMapper _mapper;

        public FilteredRemoteConfigQueryHandler(MicroscopeDbContext context, IMapper mapper)
        {
            _microscopeDbContext = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<RemoteConfigQueryResult>> Handle(FilteredRemoteConfigQuery request, CancellationToken cancellationToken)
        {
            return await this._microscopeDbContext
                .RemoteConfigs
                .ProjectTo<RemoteConfigQueryResult>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }
    }
}
