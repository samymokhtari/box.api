using box.application.Interfaces;
using box.application.Models.Response;
using Microsoft.AspNetCore.Http;

namespace box.application.Models.Request
{
    public class StorageRequest : IUseCaseRequest<StorageResponse>
    {
        /// <summary>
        /// File
        /// </summary>
        public IFormFile File { get; set; }

        /// <summary>
        /// Project
        /// </summary>
        public string ProjectCode { get; set; }

        public StorageRequest(IFormFile file, string project)
        {
            File = file;
            ProjectCode = project;
        }
    }
}