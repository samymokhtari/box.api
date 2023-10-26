using box.application.Extensions;
using box.application.Interfaces;
using box.application.Models;
using box.application.Models.Request;
using box.application.Models.Response;
using box.application.Persistance;
using Microsoft.Extensions.Configuration;
using NLog;
using System.IO;
using System.Net.Mime;

namespace box.application.UseCases
{
    public class StorageUseCase : AUseCase, IStorageUseCase
    {
        private IStorageRootPath StorageRootPath { get; }
        private IProjectRepository ProjectRepository { get; }
        private IStorageRepository StorageRepository { get; }
        private readonly string[] EXTENSION_ALLOWED = { ".jpg", ".png", ".bmp", ".pdf", ".doc", ".docx", ".txt", ".xlsx", ".csv" };
        private readonly int MAX_FILESIZE = 30000000; // In bytes

        public StorageUseCase(IConfiguration configuration, IStorageRootPath storageRootPath, IProjectRepository projectRepository, IStorageRepository storageRepository, Logger logger) 
            : base(configuration, logger)
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
            Logger.Info($"Request to store a file for project : {request.ProjectCode} ; Filename : {request.File.FileName}");

            // Check if inputs required are presents
            if (request.File == null || string.IsNullOrEmpty(request.ProjectCode))
            {
                Logger.Warn($"Request to store a file for project failed : {request.ProjectCode} ; Filename : {(request.File != null ? request.File.FileName : "No file")}");
                response.Handle(new StorageResponse(new[] { new Error("empty_request", "File and project code are mandatory") }));
                return false;
            }

            // Check extensions and file size
            if(!EXTENSION_ALLOWED.Contains(Path.GetExtension(request.File.FileName)) || request.File.Length > MAX_FILESIZE)
            {
                Logger.Warn($"Request to store a file for project failed : {request.ProjectCode} ; Filesize : {request.File.Length} ; File extension : {Path.GetExtension(request.File.FileName)}");

                response.Handle(new StorageResponse(new[] { new Error("bad_request", 
                    $"File size must be inferior than {MAX_FILESIZE} Mo.\n" +
                    $"Extensions allowed are : {string.Join(' ', EXTENSION_ALLOWED)}") }));
                return false;
            }

            // Check if project exists, 404 else
            BoxProject project = ProjectRepository.GetByCode(request.ProjectCode);
            if (project == null)
            {
                Logger.Warn($"Request to store a file for project failed because no project {request.ProjectCode} found");

               response.Handle(new StorageResponse(new[] { new Error("bad_request", "Project not found") }));
                return false;
            }

            // Store on disk
            string fileName = $"{Guid.NewGuid()}{System.IO.Path.GetExtension(request.File.FileName)}";
            string path = @$"{StorageRootPath.RootPath}{request.ProjectCode}{IStorageRootPath.DirectorySeparator}";

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
                Logger.Fatal($"Request to store a file for project failed : {request.ProjectCode} ; Filesize : {request.File.Length} ; File extension : {Path.GetExtension(request.File.FileName)}");

                response.Handle(new StorageResponse(new[] { new Error("error_while_adding_file", ex.Message) }));
                return false;
            }

            Logger.Info($"Request to store a file for project succeded : {request.ProjectCode} ;Filename : {request.File.FileName}");

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
                Logger.Warn($"Request to get a file for project failed : {request.ProjectCode} ; Filename : {request.FileName}");

                response.Handle(new StorageGetResponse(new[] { new Error("empty_request", "Request is not valid") }));
                return Task.FromResult(false);
            }

            // Check if project exists, 404 else
            BoxProject project = ProjectRepository.GetByCode(request.ProjectCode);
            if (project == null)
            {
                Logger.Warn($"Request to get a file for project failed because no project {request.ProjectCode} found");

                response.Handle(new StorageGetResponse(new[] { new Error("bad_request", "Project not found") }));
                return Task.FromResult(false);
            }

            // Check if file exists, 404 else
            if (StorageRepository.GetByName(request.FileName, project.Id) == null)
            {
                Logger.Warn($"Request to get a file for project failed : {request.ProjectCode} ; Filename : {request.FileName} not found");

                response.Handle(new StorageGetResponse(new[] { new Error("bad_request", "File not found") }));
                return Task.FromResult(false);
            }

            string path = @$"{StorageRootPath.RootPath}{request.ProjectCode}{IStorageRootPath.DirectorySeparator}{request.FileName}";

            MyFile file;
            try
            {
                byte[] byteArray = System.IO.File.ReadAllBytes(path);
                file = new(Path.GetFileName(path), byteArray);
                if(byteArray.Length == 0)
                {
                    Logger.Error($"Request to get a file for project failed : {request.ProjectCode} ; Path : {path} ; File doesn't exists");

                    response.Handle(new StorageGetResponse(new[] { new Error("error_while_reading_file", "File doesn't exists") }));
                    return Task.FromResult(false);
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"Request to get a file for project failed : {request.ProjectCode} ; Path : {path} ; Error while reading file");

                response.Handle(new StorageGetResponse(new[] { new Error("error_while_reading_file", ex.Message) }));
                return Task.FromResult(false);
            }
            Logger.Info($"Request to get a file for project succeded : {request.ProjectCode} ; Path : {path}");
            
            response.Handle(new StorageGetResponse(file, true, $"File {request.FileName} read successfully"));
            return Task.FromResult(true);
        }
    }
}