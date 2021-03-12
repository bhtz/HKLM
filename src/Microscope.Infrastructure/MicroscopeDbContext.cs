using System.Reflection;
using Microscope.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Microscope.Infrastructure
{
    public class MicroscopeDbContext : DbContext
    {
        public virtual DbSet<Analytic> Analytics { get; set; }
        public virtual DbSet<RemoteConfig> RemoteConfigs { get; set; }
        
        public MicroscopeDbContext(DbContextOptions<MicroscopeDbContext> options) : base(options)
        {
            
        }

        public void Migrate()
        {
            this.Database.Migrate();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.HasDefaultSchema("mcsp_common");
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(builder);
        }
    }
}
