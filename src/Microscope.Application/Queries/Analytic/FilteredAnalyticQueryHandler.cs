using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microscope.Application.Core.Queries.Analytic;
using Microscope.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Microscope.Application.Features.Analytic.Queries
{
    public class FilteredAnalyticQueryHandler : IRequestHandler<FilteredAnalyticQuery, IEnumerable<AnalyticQueryResult>>
    {
        private readonly MicroscopeDbContext _microscopeDbContext;
        private readonly IMapper _mapper;

        public FilteredAnalyticQueryHandler(MicroscopeDbContext context, IMapper mapper)
        {
            _microscopeDbContext = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<AnalyticQueryResult>> Handle(FilteredAnalyticQuery request, CancellationToken cancellationToken)
        {
            return await this._microscopeDbContext
                .Analytics
                .ProjectTo<AnalyticQueryResult>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }
    }
}
