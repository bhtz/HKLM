using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microscope.Domain.Aggregates.AnalyticAggregate;
using Microscope.Domain.Entities;
using Microscope.Domain.SharedKernel;

namespace Microscope.Infrastructure.Repositories.Analytics
{
    public class AnalyticRepository : EFRepositoryBase<Analytic>, IAnalyticRepository
    {
        public AnalyticRepository(MicroscopeDbContext context) : base(context)
        {
            
        }  
    }
}
