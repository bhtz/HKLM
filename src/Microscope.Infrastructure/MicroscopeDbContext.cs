using System.Reflection;
using Microscope.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Microscope.Infrastructure
{
    public class MicroscopeDbContext : DbContext
    {
        public virtual DbSet<Analytic> Analytics { get; set; }
        public virtual DbSet<RemoteConfig> RemoteConfigs { get; set; }
        
        public MicroscopeDbContext(DbContextOptions options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(builder);
        }
    }
}
