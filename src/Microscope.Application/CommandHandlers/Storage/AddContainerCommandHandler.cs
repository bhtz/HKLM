using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microscope.Application.Core.Commands.Storage;
using Microscope.Domain.Entities;
using Microscope.Domain.Repositories;

namespace Microscope.Application.Commands.Storage
{
    public class AddContainerCommandHandler : IRequestHandler<AddContainerCommand, string>
    {
        private readonly IContainerRepository _repository;
        private readonly IMapper _mapper;

        public AddContainerCommandHandler(IContainerRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<string> Handle(AddContainerCommand command, CancellationToken cancellationToken)
        {
            Container entity = Container.NewContainer(command.Name);
            await this._repository.AddAsync(entity);
            await this._repository.UnitOfWork.SaveChangesAsync();
            return command.Name;
        }
    }
}
