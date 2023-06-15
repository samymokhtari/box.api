using box.application.Interfaces;
using box.application.Models.Paths;
using box.application.Models.Response;
using box.application.UseCases;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NLog;

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

            // Logging
            var config = new NLog.Config.LoggingConfiguration();
            var fluentdTarget = new NLog.Targets.Fluentd();

            fluentdTarget.Layout = new NLog.Layouts.SimpleLayout("${longdate}|${level}|${callsite}|${logger}|${message}");
            fluentdTarget.Host = "fluent-bit";
            fluentdTarget.Port = 24224;
            fluentdTarget.Tag = "hdfs.nlog.test";
            config.AddTarget("fluentd", fluentdTarget);
            config.LoggingRules.Add(new NLog.Config.LoggingRule("box", LogLevel.Debug, fluentdTarget));
            var loggerFactory = new LogFactory(config);
            var logger = loggerFactory.GetLogger("box");
            logger.Info("Application Starting...");

            services.AddSingleton(logger);

            return services;
        }
    }
}