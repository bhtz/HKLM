using System;
using FluentValidation;
using MediatR;

namespace Microscope.Application.Core.Commands.Analytic
{
    public class DeleteAnalyticCommand : IRequest<Guid>
    {
        public Guid Id { get; set; }
    }

    public class DeleteAnalyticCommandValidator : AbstractValidator<DeleteAnalyticCommand>
    {
        public DeleteAnalyticCommandValidator()
        {
            RuleFor(v => v.Id).NotEmpty();
        }
    }
}