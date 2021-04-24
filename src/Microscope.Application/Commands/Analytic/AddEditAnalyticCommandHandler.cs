using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microscope.Application.Core.Commands.Analytic;
using Microscope.Infrastructure;

namespace Microscope.Application.Features.Analytic.Commands
{
    public class AddEditAnalyticCommandHandler : IRequestHandler<AddEditAnalyticCommand, Guid>
    {
        private readonly MicroscopeDbContext _context;
        private readonly IMapper _mapper;

        public AddEditAnalyticCommandHandler(MicroscopeDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Guid> Handle(AddEditAnalyticCommand request, CancellationToken cancellationToken)
        {
            Microscope.Domain.Entities.Analytic entity;

            if(request.Id == Guid.Empty)
            {
                entity = this._mapper.Map<Microscope.Domain.Entities.Analytic>(request);
                entity.Id = Guid.NewGuid();
                
                this._context.Analytics.Add(entity);
                await this._context.SaveChangesAsync(cancellationToken);
            }
            else
            {
                entity = this._context.Analytics.FirstOrDefault(x => x.Id == request.Id);
                entity.Key = request.Key ?? entity.Key;
                entity.Dimension = request.Dimension ?? entity.Dimension;
                this._context.Update(entity);
                await this._context.SaveChangesAsync();
            }

            return entity.Id;
        }
    }
}
