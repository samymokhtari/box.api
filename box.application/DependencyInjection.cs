using AutoMapper;
using box.application.Interfaces;
using box.application.Models.Paths;
using box.application.Models.Response;
using box.application.UseCases;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace box.application
{
    public static class DependencyInjection
    {
        public static IServiceCollection ApplicationPersistance(this IServiceCollection services, IConfiguration configuration)
        {
            // needed to load configuration from appsettings.json
            services.AddOptions();

            // Singletons
            services.AddSingleton(configuration);

            // Others
            services.AddScoped<IStorageRootPath, StorageRootPath>();
            services.AddScoped<EmptyResponse, EmptyResponse>();

            // UseCases
            services.AddScoped<IStorageUseCase, StorageUseCase>();
            services.AddScoped<IProjectUseCase, ProjectUseCase>();


            return services;
        }
    }
}