using Ardalis.GuardClauses;
using box.application.Interfaces;
using box.application.Models.Response;

namespace box.application.Models.Request
{
    public class ProjectRequest : IUseCaseRequest<ProjectResponse> , IUseCaseRequest<EmptyResponse>
    {
        /// <summary>
        /// Project Name
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// Project Name
        /// </summary>
        public string ProjectCode { get; set; }

        public ProjectRequest(string projectName, string projectCode) : this(projectCode)
        {
            Guard.Against.NullOrEmpty(projectName, nameof(projectName));
            ProjectName = projectName;
        }

        public ProjectRequest(string projectCode)
        {
            Guard.Against.NullOrEmpty(projectCode, nameof(projectCode));
            Guard.Against.InvalidFormat(projectCode, nameof(projectCode), "[A-Z]*");

            ProjectCode = projectCode;
            ProjectName = string.Empty;
        }
    }
}