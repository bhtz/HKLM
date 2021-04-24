using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microscope.Application.Core.Commands.RemoteConfig;
using Microscope.Domain.Entities;
using Microscope.Infrastructure;

namespace Microscope.Application.Commands.RemoteConfig
{
    public class AddEditRemoteConfigCommandHandler : IRequestHandler<AddEditRemoteConfigCommand, Guid> 
    {
        private readonly MicroscopeDbContext _context;
        private readonly IMapper _mapper;
        
        public AddEditRemoteConfigCommandHandler(MicroscopeDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Guid> Handle(AddEditRemoteConfigCommand request, CancellationToken cancellationToken)
        {
            Microscope.Domain.Entities.RemoteConfig entity;

            if(request.Id == Guid.Empty)
            {
                entity = this._mapper.Map<Microscope.Domain.Entities.RemoteConfig>(request);
                entity.Id = Guid.NewGuid();
                
                this._context.RemoteConfigs.Add(entity);
                await this._context.SaveChangesAsync(cancellationToken);
            }
            else
            {
                entity = this._context.RemoteConfigs.FirstOrDefault(x => x.Id == request.Id);
                entity.Key = request.Key ?? entity.Key;
                entity.Dimension = request.Dimension ?? entity.Dimension;

                this._context.Update(entity);
                await this._context.SaveChangesAsync();
            }

            return entity.Id;
        }
    }
}
