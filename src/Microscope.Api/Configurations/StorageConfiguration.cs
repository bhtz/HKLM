using Microscope.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Microscope.Configurations
{
    public static class StorageConfiguration
    {
        public static IServiceCollection AddStorageConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            StorageOptions options = new StorageOptions();
            IConfigurationSection section = configuration.GetSection("Storage");
            
            section.Bind(options);
            services.Configure<StorageOptions>(section);

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