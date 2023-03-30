using box.application.Interfaces;
using box.application.Models.Response;

namespace box.application.Models.Request
{
    public class StorageGetRequest : IUseCaseRequest<StorageGetResponse>
    {
        /// <summary>
        /// project code
        /// </summary>
        public string ProjectCode { get; set; }

        /// <summary>
        /// filename
        /// </summary>
        public string FileName { get; set; }

        public StorageGetRequest(string projectCode, string fileName)
        {
            ProjectCode = projectCode;
            FileName = fileName;
        }
    }
}