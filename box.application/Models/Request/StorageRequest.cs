using box.application.Interfaces;
using box.application.Models.Response;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace box.application.Models.Request
{
    public class StorageRequest : IUseCaseRequest<StorageResponse>
    {
        /// <summary>
        /// File
        /// </summary>
        public IFormFile File { get; set; }

        /// <summary>
        /// Project id
        /// </summary>
        public string ProjectID { get; set; }

        public StorageRequest(IFormFile file, string projectID)
        {
            File = file;
            ProjectID = projectID;
        }
    }
}