using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microscope.Application.Core.Commands.Analytic;
using Microscope.Application.Core.Commands.Storage;
using Microscope.Infrastructure;

namespace Microscope.Application.Commands.Storage
{
    public class AddContainerCommandHandler : IRequestHandler<AddContainerCommand, string>
    {
        private readonly MicroscopeDbContext _context;
        private readonly IMapper _mapper;

        public AddContainerCommandHandler(MicroscopeDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<string> Handle(AddContainerCommand command, CancellationToken cancellationToken)
        {
    
            return await Task.FromResult(string.Empty);
        }
    }
}
