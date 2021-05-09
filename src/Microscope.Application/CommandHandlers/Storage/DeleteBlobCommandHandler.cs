using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microscope.Application.Features.Commands.Storage;
using Microscope.Domain.Services;

namespace Microscope.Application.CommandHandlers.Storage
{
    public class DeleteBlobCommandHandler : IRequestHandler<DeleteBlobCommand, bool>
    {
        private readonly IStorageService _storageService;

        public DeleteBlobCommandHandler(IStorageService storageService)
        {
            _storageService = storageService;
        }

        public async Task<bool> Handle(DeleteBlobCommand request, CancellationToken cancellationToken)
        {
            await this._storageService.DeleteBlobAsync(request.ContainerName, request.Name);
            return true;
        }
    }
}
