using System;
using FluentValidation;
using MediatR;

namespace Microscope.Application.Core.Commands.Analytic
{
    public class AddEditAnalyticCommand : IRequest<Guid>
    {
        public Guid Id { get; set; }
        public string Key { get; set; }
        public string Dimension { get; set; }
    }

    public class AddEditAnalyticCommandValidator : AbstractValidator<AddEditAnalyticCommand>
    {
        public AddEditAnalyticCommandValidator()
        {
            RuleFor(v => v.Id).NotEmpty();
            RuleFor(v => v.Key).NotEmpty();
            RuleFor(v => v.Dimension).NotEmpty();
        }
    }
}