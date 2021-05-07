using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microscope.Application.Core.Commands.RemoteConfig;
using Microscope.Domain.Aggregates.RemoteConfigAggregate;

namespace Microscope.Application.Commands.RemoteConfig
{
    public class DeleteRemoteConfigCommandHandler : IRequestHandler<DeleteRemoteConfigCommand, Guid> 
    {
        private readonly IRemoteConfigRepository _repository;
        private readonly IMapper _mapper;

        public DeleteRemoteConfigCommandHandler(IRemoteConfigRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;    
        }

        public async Task<Guid> Handle(DeleteRemoteConfigCommand request, CancellationToken cancellationToken)
        {
            var entity = this._repository.Entities.FirstOrDefault(x => x.Id == request.Id);
            await this._repository.DeleteAsync(entity);
            return request.Id;
        }
    }
}
