using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microscope.Application.Core.Commands.RemoteConfig;
using Microscope.Infrastructure;

namespace Microscope.Application.Commands.RemoteConfig
{
    public class DeleteRemoteConfigCommandHandler : IRequestHandler<DeleteRemoteConfigCommand, Guid> 
    {
        private readonly MicroscopeDbContext _context;
        private readonly IMapper _mapper;

        public DeleteRemoteConfigCommandHandler(MicroscopeDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;    
        }

        public Task<Guid> Handle(DeleteRemoteConfigCommand request, CancellationToken cancellationToken)
        {
            var entity = this._context.RemoteConfigs.FirstOrDefault(x => x.Id == request.Id);
            this._context.RemoteConfigs.Remove(entity);
            return Task.FromResult(request.Id);
        }
    }
}
