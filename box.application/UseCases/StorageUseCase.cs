using box.application.Extensions;
using box.application.Interfaces;
using box.application.Models;
using box.application.Models.Request;
using box.application.Models.Response;
using Microsoft.Extensions.Configuration;

namespace box.application.UseCases
{
    public class StorageUseCase : AUseCase, IStorageUseCase
    {
        private IStorageRootPath StorageRootPath { get; }
        private IProjectUseCase ProjectUseCase { get; }

        public StorageUseCase(IConfiguration configuration, IStorageRootPath storageRootPath, IProjectUseCase projectUseCase) : base(configuration)
        {
            StorageRootPath = storageRootPath;
            ProjectUseCase = projectUseCase;
        }

        /// <summary>
        /// Store a file
        /// </summary>
        /// <param name="request"></param>
        /// <returns>boolean success</returns>
        public async Task<bool> HandleAsync(StorageRequest request, IOutputPort<StorageResponse> response)
        {
            if (request.File == null)
            {
                response.Handle(new StorageResponse(new[] { new Error("empty_request", "Request is not valid") }));
                return false;
            }

            // Check if project exists, 404 else

            // Store on disk
            string path = @$"{StorageRootPath.RootPath}{StorageRootPath.DirectorySeparator}{request.Project}{StorageRootPath.DirectorySeparator}";
            Task saveImageTask = System.IO.File.WriteAllBytesAsync(path, request.File.GetByteArray());

            // Store in database
            //Task processTask = ProjectUseCase.HandleAsync();


            // Asynchronously wait for both tasks to complete.
            //await Task.WhenAll(saveImageTask, processTask);

            return true;
        }
    }
}