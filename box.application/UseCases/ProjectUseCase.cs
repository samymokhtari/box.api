using box.application.Interfaces;
using box.application.Models;
using box.application.Models.Request;
using box.application.Models.Response;
using box.application.Persistance;
using Microsoft.Extensions.Configuration;
using NLog;

namespace box.application.UseCases
{
    public class ProjectUseCase : AUseCase, IProjectUseCase
    {
        private readonly IProjectRepository _projectRepository;

        public ProjectUseCase(IConfiguration configuration, IProjectRepository projectRepository, Logger logger) : base(configuration, logger)
        {
            _projectRepository = projectRepository;
        }

        /// <summary>
        /// Get a project
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
        /// <returns>success or not</returns>
        public Task<bool> HandleAsync(ProjectRequest request, IOutputPort<ProjectResponse> response)
        {
            Logger.Info($"Get a projet by the code : {request.ProjectCode}");

            BoxProject project = _projectRepository.GetByCode(request.ProjectCode);

            if (project == null)
            {
                Logger.Error($"Failed to get following project : {request.ProjectCode}");

                response.Handle(new ProjectResponse(new[] { new Error("bad_request", "Project not found") }));
                return Task.FromResult(false);
            }

            response.Handle(new ProjectResponse(project, true, project.Name));
            return Task.FromResult(true);
        }

        /// <summary>
        /// Add a project
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
        /// <returns>success or not</returns>
        public async Task<bool> HandleAsync(ProjectRequest request, IOutputPort<EmptyResponse> response)
        {
            Logger.Info($"Post project with following informations : {request.ProjectCode} ; {request.ProjectName}");

            if (string.IsNullOrEmpty(request.ProjectName) || string.IsNullOrEmpty(request.ProjectCode))
            {
                Logger.Error($"Failed to add following project : {request.ProjectCode} ; {request.ProjectName}");

                response.Handle(new EmptyResponse(new[] { new Error("bad_request", "Parameters are mandatory") }));
                return false;
            }

            await _projectRepository.AddAsync(new BoxProject(
                request.ProjectName, request.ProjectCode));

            Logger.Info($"New project added : {request.ProjectCode} ; {request.ProjectName}");

            response.Handle(new EmptyResponse());
            return true;
        }
    }
}