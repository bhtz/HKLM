using System;
using FluentValidation;

namespace Microscope.Application.Core.Commands.RemoteConfig
{
    public class DeleteRemoteConfigCommand
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
