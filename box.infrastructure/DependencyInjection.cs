using AutoMapper;
using box.application.Persistance;
using box.infrastructure.Data.Entities;
using box.infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace box.infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection InfrastructurePersistence(this IServiceCollection services, IConfiguration configuration, Type program)
        {
            // Repositories
            services.AddTransient<IProjectRepository, ProjectRepository>();

            services.AddDbContext<BoxContext, BoxContext>();
            services.AddDbContext<BoxContext>(options => options.UseSqlServer(configuration.GetConnectionString("ConnStr")));

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