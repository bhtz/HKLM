using System;
using FluentValidation;
using MediatR;

namespace Microscope.Application.Core.Commands.RemoteConfig
{
    public class AddRemoteConfigCommand : IRequest<Guid>
    {
        public string Key { get; set; }
        public string Dimension { get; set; }
    }

    public class AddRemoteConfigCommandValidator : AbstractValidator<AddRemoteConfigCommand>
    {
        public AddRemoteConfigCommandValidator()
        {
            RuleFor(v => v.Key).NotEmpty();
            RuleFor(v => v.Dimension).NotEmpty();
        }
    }
}
