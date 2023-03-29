using box.application.Models.Response;
using box.application.Models.Request;

namespace box.application.Interfaces
{
    public interface IProjectUseCase : IUseCaseRequestHandler<ProjectRequest, ProjectResponse>, IUseCaseRequestHandler<ProjectRequest, EmptyResponse>
    {
    }
}