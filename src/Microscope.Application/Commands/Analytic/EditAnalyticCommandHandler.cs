using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microscope.Application.Core.Commands.Analytic;
using Microscope.Infrastructure;

namespace Microscope.Application.Commands.AnalyticHandlers
{
    public class EditAnalyticCommandHandler : IRequestHandler<EditAnalyticCommand, Guid>
    {
        private readonly MicroscopeDbContext _context;
        private readonly IMapper _mapper;

        public EditAnalyticCommandHandler(MicroscopeDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Guid> Handle(EditAnalyticCommand command, CancellationToken cancellationToken)
        {
            var entity = this._context.Analytics.FirstOrDefault(x => x.Id == command.Id);

            entity.Update(command.Key, command.Dimension);
            
            this._context.Update(entity);
            await this._context.SaveChangesAsync();

            return entity.Id;
        }
    }
}
