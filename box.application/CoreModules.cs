using box.application.Interfaces;
using box.application.UseCases;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace box.application
{
    public static class CoreModules
    {
        public static IServiceCollection ImplementPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            // Singletons
            services.AddSingleton(configuration);

            // UseCases
            services.AddScoped<IStorageUseCase, StorageUseCase>();

            return services;
        }
    }
}