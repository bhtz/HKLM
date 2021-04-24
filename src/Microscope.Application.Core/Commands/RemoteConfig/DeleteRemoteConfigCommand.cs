using System;
using FluentValidation;
using MediatR;

namespace Microscope.Application.Core.Commands.RemoteConfig
{
    public class DeleteRemoteConfigCommand : IRequest<Guid>
    {
        public Guid Id { get; set; }
    }

    public class DeleteRemoteConfigCommandValidator : AbstractValidator<DeleteRemoteConfigCommand>
    {
        public DeleteRemoteConfigCommandValidator()
        {
            RuleFor(v => v.Id).NotEmpty();
        }
    }
}
