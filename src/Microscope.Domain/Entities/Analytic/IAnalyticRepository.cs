using Microscope.Domain.Entities;
using Microscope.Domain.Interfaces;

namespace Microscope.Domain.Aggregates.AnalyticAggregate
{
    public interface IAnalyticRepository : IRepository<Analytic>
    {
        IUnitOfWork UnitOfWork { get; }
    }
}
