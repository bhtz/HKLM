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
    public class EditRemoteConfigCommandHandler : IRequestHandler<EditRemoteConfigCommand, Guid>
    {
        private readonly MicroscopeDbContext _context;
        private readonly IMapper _mapper;

        public EditRemoteConfigCommandHandler(MicroscopeDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Guid> Handle(EditRemoteConfigCommand command, CancellationToken cancellationToken)
        {

            var entity = this._context.RemoteConfigs.SingleOrDefault(x => x.Id == command.Id);
            entity.Update(command.Key,command.Dimension);

            this._context.Update(entity);
            await this._context.SaveChangesAsync();

            return entity.Id;
        }
    }
}
