using box.application.Extensions;
using box.application.Interfaces;
using box.application.Models;
using box.application.Models.Request;
using box.application.Models.Response;
using box.application.Persistance;
using Microsoft.Extensions.Configuration;

namespace box.application.UseCases
{
    public class StorageUseCase : AUseCase, IStorageUseCase
    {
        private IStorageRootPath StorageRootPath { get; }
        private IProjectRepository ProjectRepository { get; }
        private IStorageRepository StorageRepository { get; }

        public StorageUseCase(IConfiguration configuration, IStorageRootPath storageRootPath, IProjectRepository projectRepository, IStorageRepository storageRepository) : base(configuration)
        {
            StorageRootPath = storageRootPath;
            ProjectRepository = projectRepository;
            StorageRepository = storageRepository;
        }

        /// <summary>
        /// Store a file
        /// </summary>
        /// <param name="request"></param>
        /// <returns>boolean success</returns>
        public async Task<bool> HandleAsync(StorageRequest request, IOutputPort<StorageResponse> response)
        {
            if (request.File == null || string.IsNullOrEmpty(request.ProjectCode))
            {
                response.Handle(new StorageResponse(new[] { new Error("empty_request", "Request is not valid") }));
                return false;
            }

            // Check if project exists, 404 else
            BoxProject project = ProjectRepository.GetByCode(request.ProjectCode);
            if (project == null)
            {
                response.Handle(new StorageResponse(new[] { new Error("bad_request", "Project not found") }));
                return false;
            }

            // Store on disk
            string fileName = $"{Guid.NewGuid()}{System.IO.Path.GetExtension(request.File.FileName)}";
            string path = @$"{StorageRootPath.RootPath}{request.ProjectCode}{StorageRootPath.DirectorySeparator}";

            // Create Path if doesn't exists
            System.IO.Directory.CreateDirectory(path);

            path += fileName;

            Task saveImageTask = System.IO.File.WriteAllBytesAsync(path, request.File.GetByteArray());

            // Store in database
            Task processTask = StorageRepository.AddAsync(new BoxFile(fileName, project.Id));


            // Asynchronously wait for both tasks to complete.
            try
            {
                await Task.WhenAll(saveImageTask, processTask);
            }catch(Exception ex)
            {
                response.Handle(new StorageResponse(new[] { new Error("error_while_adding_file", ex.Message) }));
                return false;
            }

            response.Handle(new StorageResponse(fileName, true, $"File Added Successfuly on project {request.ProjectCode}"));
            return true;
        }

        /// <summary>
        /// Get a file
        /// </summary>
        /// <param name="message"></param>
        /// <param name="outputPort"></param>
        /// <returns>boolean success</returns>
        public Task<bool> HandleAsync(StorageGetRequest request, IOutputPort<StorageGetResponse> response)
        {
            // Check inputs
            if (string.IsNullOrEmpty(request.FileName) || string.IsNullOrEmpty(request.ProjectCode))
            {
                response.Handle(new StorageGetResponse(new[] { new Error("empty_request", "Request is not valid") }));
                return Task.FromResult(false);
            }

            // Check if project exists, 404 else
            BoxProject project = ProjectRepository.GetByCode(request.ProjectCode);
            if (project == null)
            {
                response.Handle(new StorageGetResponse(new[] { new Error("bad_request", "Project not found") }));
                return Task.FromResult(false);
            }

            // Check if file exists, 404 else
            if (StorageRepository.GetByName(request.FileName, project.Id) == null)
            {
                response.Handle(new StorageGetResponse(new[] { new Error("bad_request", "File not found") }));
                return Task.FromResult(false);
            }

            string path = @$"{StorageRootPath.RootPath}{request.ProjectCode}{StorageRootPath.DirectorySeparator}{request.FileName}";

            ByteArrayContent byteArrayContent;
            try
            {
                byteArrayContent = new ByteArrayContent(System.IO.File.ReadAllBytes(path));
            }catch(Exception ex)
            {
                response.Handle(new StorageGetResponse(new[] { new Error("error_while_reading_file", ex.Message) }));
                return Task.FromResult(false);
            }

            response.Handle(new StorageGetResponse(byteArrayContent, true, $"File {request.FileName} read successfully"));
            return Task.FromResult(true);
        }
    }
}