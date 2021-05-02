using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microscope.Application.Core.Commands.Analytic;
using Microscope.Domain.Entities;
using Microscope.Infrastructure;

namespace Microscope.Application.Commands.AnalyticHandlers
{
    public class AddAnalyticCommandHandler : IRequestHandler<AddAnalyticCommand, Guid>
    {
        private readonly MicroscopeDbContext _context;
        private readonly IMapper _mapper;

        public AddAnalyticCommandHandler(MicroscopeDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Guid> Handle(AddAnalyticCommand command, CancellationToken cancellationToken)
        {
            var entity = Analytic.NewAnalytic(Guid.NewGuid(),command.Key,command.Dimension);
            this._context.Analytics.Add(entity);
            await this._context.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }
    }
}
