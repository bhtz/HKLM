using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microscope.Domain.Services;
using Microscope.Infrastructure.Storage;

namespace Microscope.Infrastructure
{
    public static class DependencyInjection
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
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
                        options.UseNpgsql(connectionString, o => o.MigrationsAssembly(assemblyName));
                        break;

                    case "mssql":
                        options.UseSqlServer(connectionString, o => o.MigrationsAssembly(assemblyName));
                        break;

                    default:
                        options.UseInMemoryDatabase(brandName);
                        break;
                }
            });

            return services;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddStorageConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            StorageOptions options = new StorageOptions();
            IConfigurationSection section = configuration.GetSection("Storage");
            section.Bind(options);

            switch (options.Adapter)
            {
                case "filesystem":
                    services.AddScoped<IStorageService, FileSystemStorageService>();
                    break;

                case "azure":
                    services.AddScoped<IStorageService, BlobStorageService>();
                    break;

                case "aws":
                    services.AddScoped<IStorageService, AwsStorageService>();
                    break;

                case "minio":
                    services.AddScoped<IStorageService, MinioStorageService>();
                    break;

                default:
                    services.AddScoped<IStorageService, MinioStorageService>();
                    break;
            }

            return services;
        }
    }
}