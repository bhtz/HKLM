using System;
using FluentValidation;
using MediatR;

namespace Microscope.Application.Core.Commands.RemoteConfig
{
    public class AddEditRemoteConfigCommand : IRequest<Guid>
    {
        public Guid Id { get; set; }
        public string Key { get; set; }
        public string Dimension { get; set; }
    }

    public class AddEditRemoteConfigCommandValidator : AbstractValidator<AddEditRemoteConfigCommand>
    {
        public AddEditRemoteConfigCommandValidator()
        {
            RuleFor(v => v.Id).NotEmpty();
            RuleFor(v => v.Key).NotEmpty();
            RuleFor(v => v.Dimension).NotEmpty();
        }
    }
}
