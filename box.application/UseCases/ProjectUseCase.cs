using box.application.Interfaces;
using box.application.Models;
using box.application.Models.Request;
using box.application.Models.Response;
using box.application.Persistance;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace box.application.UseCases
{
    public class ProjectUseCase : AUseCase, IProjectUseCase
    {
        private readonly IProjectRepository _projectRepository;
        public ProjectUseCase(IConfiguration configuration, IProjectRepository projectRepository) : base(configuration)
        {
            _projectRepository = projectRepository;
        }
        /// <summary>
        /// Get a project
        /// </summary>
        /// <param name="message"></param>
        /// <param name="response"></param>
        /// <returns>success or not</returns>
        public async Task<bool> HandleAsync(ProjectRequest message, IOutputPort<ProjectResponse> response)
        {
            BoxProject project = _projectRepository.GetByCode(message.ProjectCode);
            
            if (project == null)
            {
                response.Handle(new ProjectResponse(new[] { new Error("bad_request", "Project not found") }));
                return false;
            }

            response.Handle(new ProjectResponse(project, true, project.Name));
            return true;
        }

        /// <summary>
        /// Add a project
        /// </summary>
        /// <param name="message"></param>
        /// <param name="response"></param>
        /// <returns>success or not</returns>
        public async Task<bool> HandleAsync(ProjectRequest message, IOutputPort<EmptyResponse> response)
        {
            if(message == null || string.IsNullOrEmpty(message.ProjectName) || string.IsNullOrEmpty(message.ProjectCode))
            {
                response.Handle(new EmptyResponse(new[] { new Error("bad_request", "Parameters are mandatory") }));
                return false;
            }

            await _projectRepository.AddAsync(new BoxProject(
                message.ProjectName, message.ProjectCode));

            response.Handle(new EmptyResponse());
            return true;
        }
    }
}
