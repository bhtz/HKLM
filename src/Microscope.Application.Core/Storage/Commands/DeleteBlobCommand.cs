using System;
using FluentValidation;
using MediatR;

namespace Microscope.Application.Features.Storage.Commands
{
    public class DeleteBlobCommand : IRequest<bool>
    {
        public string Name { get; set; }
        public string ContainerName { get; set; }
    }

    public class DeleteBlobCommandValidator : AbstractValidator<DeleteBlobCommand>
    {
        public DeleteBlobCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.ContainerName).NotEmpty();
        }
    }
}
