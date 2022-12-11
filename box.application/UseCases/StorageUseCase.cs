using box.application.Interfaces;
using box.application.Models;
using box.application.Models.Request;
using box.application.Models.Response;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace box.application.UseCases
{
    public class StorageUseCase : AUseCase, IStorageUseCase
    {
        IStorageRootPath StorageRootPath { get; set; }
        public StorageUseCase(IConfiguration configuration, IStorageRootPath storageRootPath) : base(configuration)
        {
            StorageRootPath = storageRootPath;
        }

        /// <summary>
        /// Store a file
        /// </summary>
        /// <param name="request"></param>
        /// <returns>boolean success</returns>
        public async Task<bool> HandleAsync(StorageRequest request,IOutputPort<StorageResponse> response)
        {
            if (request.File == null)
            {
                response.Handle(new StorageResponse(new[] { new Error("empty_request", "Request is not valid") }));
                return false;
            }

            // Store on disk
            string path = StorageRootPath.RootPath;

            // Store in database

            return true;
        }
    }
}