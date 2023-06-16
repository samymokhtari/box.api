using Microsoft.Extensions.Configuration;
using NLog;

namespace box.application.UseCases
{
    public abstract class AUseCase
    {
        public IConfiguration Configuration { get; }

        public Logger Logger { get; }

        public AUseCase(IConfiguration configuration, Logger logger)
        {
            Configuration = configuration;
            Logger = logger;
        }
    }
}