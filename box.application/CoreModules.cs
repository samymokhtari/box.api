using AutoMapper;
using box.application.Interfaces;
using box.application.UseCases;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace box.application
{
    public static class CoreModules
    {
        public static IServiceCollection ImplementPersistence(this IServiceCollection services, IConfiguration configuration, Type program)
        {
            // Singletons
            services.AddSingleton(configuration);

            // UseCases
            services.AddScoped<IStorageUseCase, StorageUseCase>();

            // AutoMapper
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new Profiles());
            });
            
            var mapper = config.CreateMapper();

            services.AddSingleton(mapper);
            services.AddAutoMapper(program);

            return services;
        }
    }
}