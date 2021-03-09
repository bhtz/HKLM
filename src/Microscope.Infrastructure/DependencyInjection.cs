using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microscope.Infrastructure.Persistence;

namespace Microscope.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var brandName = "Microscope";
            var provider = configuration.GetValue<string>("DatabaseProvider");
            var connectionString = configuration.GetConnectionString(brandName);
            var assemblyName = typeof(MicroscopeDbContext).Assembly.FullName;

            services.AddDbContext<MicroscopeDbContext>(options => 
            {
                switch (provider)
                {
                    case "postgres":
                        options.UseNpgsql(connectionString, o =>
                        {
                            o.MigrationsAssembly(assemblyName);
                        });
                        break;

                    case "mssql":
                        options.UseSqlServer(connectionString, o =>
                        {
                            o.MigrationsAssembly(assemblyName);
                        });
                        break;

                    default:
                        options.UseInMemoryDatabase(brandName);
                        break;
                }
            });

            return services;
        }
    }
}