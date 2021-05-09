using System;
using System.IO;
using FluentValidation;
using MediatR;

namespace Microscope.Application.Features.Commands.Storage
{
    public class UploadBlobCommand : IRequest<bool>
    {
        public string Name { get; set; }
        public string ContainerName { get; set; }
        public Stream Data { get; set; }
    }

    public class UploadBlobCommandValidator : AbstractValidator<UploadBlobCommand>
    {
        public UploadBlobCommandValidator()
        {
            RuleFor(v => v.Name).NotEmpty();
            RuleFor(v => v.Data).NotEmpty();
        }
    }
}
