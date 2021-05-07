using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microscope.Application.Core.Commands.Analytic;
using Microscope.Domain.Aggregates.AnalyticAggregate;

namespace Microscope.Application.Commands.AnalyticHandlers
{
    public class DeleteAnalyticCommandHandler : IRequestHandler<DeleteAnalyticCommand, Guid>
    {
        private readonly IAnalyticRepository _repository;
        private readonly IMapper _mapper;
        
        public DeleteAnalyticCommandHandler(IAnalyticRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Guid> Handle(DeleteAnalyticCommand request, CancellationToken cancellationToken)
        {
            var entity = this._repository.Entities.FirstOrDefault(x => x.Id == request.Id);
            await this._repository.DeleteAsync(entity);
            return entity.Id;
        }
    }
}


