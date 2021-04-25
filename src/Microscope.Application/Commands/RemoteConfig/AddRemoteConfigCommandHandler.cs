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
    public class AddRemoteConfigCommandHandler : IRequestHandler<AddRemoteConfigCommand, Guid>
    {
        private readonly MicroscopeDbContext _context;
        private readonly IMapper _mapper;

        public AddRemoteConfigCommandHandler(MicroscopeDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Guid> Handle(AddRemoteConfigCommand command, CancellationToken cancellationToken)
        {

            var entity = Microscope.Domain.Entities.RemoteConfig.NewRemoteConfig(Guid.NewGuid(), command.Key, command.Dimension);

            this._context.RemoteConfigs.Add(entity);
            await this._context.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }
    }
}
