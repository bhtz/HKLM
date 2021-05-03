using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microscope.Application.Core.Commands.Analytic;
using Microscope.Infrastructure;

namespace Microscope.Application.Commands.AnalyticHandlers
{
    public class DeleteAnalyticCommandHandler : IRequestHandler<DeleteAnalyticCommand, Guid>
    {
        private readonly MicroscopeDbContext _context;
        private readonly IMapper _mapper;
        
        public DeleteAnalyticCommandHandler(MicroscopeDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public Task<Guid> Handle(DeleteAnalyticCommand request, CancellationToken cancellationToken)
        {
            var entity = this._context.Analytics.FirstOrDefault(x => x.Id == request.Id);
            this._context.Analytics.Remove(entity);
            return Task.FromResult(request.Id);
        }
    }
}


