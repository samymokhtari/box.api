using Microsoft.Extensions.Configuration;

namespace box.application.UseCases
{
    public abstract class AUseCase
    {
        public IConfiguration Configuration { get; }

        public AUseCase(IConfiguration configuration)
        {
            Configuration = configuration;
        }
    }
}