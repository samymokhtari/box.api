using box.application.Interfaces;
using box.application.Models;
using box.application.Models.Paths;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using NLog;
using System.Security.AccessControl;
using static Google.Rpc.Context.AttributeContext.Types;

namespace box.application.UseCases
{
    public class GoogleCloudStorageUseCase : AUseCase
    {
        private readonly GoogleCredential googleCredential;
        private readonly StorageClient storageClient;
        private readonly UrlSigner urlSigner;
        private readonly string bucketName;
        private readonly string ENV_GCP_KEY_NAME = "GOOGLE_APPLICATION_CREDENTIALS";

        public GoogleCloudStorageUseCase(IConfiguration configuration, Logger logger) : base(configuration, logger)
        {
            var gcpSecret = configuration.GetValue<string>(ENV_GCP_KEY_NAME);
            if (!string.IsNullOrEmpty(gcpSecret))
            {
                var cr = JsonConvert.DeserializeObject<GoogleCredentialServiceAccount>(gcpSecret);
                var jsonString = JsonConvert.SerializeObject(cr, Formatting.None);
                googleCredential = GoogleCredential.FromJson(jsonString);
            }
            else if (Environment.GetEnvironmentVariable("ENV") == "production")
            {
                logger.Info("Use Default Auth GCP Internal Service");
                googleCredential = GoogleCredential.GetApplicationDefault(); // Default Auth GCP Service
            }
            else
            {
                throw new ArgumentException("Google Credentials not found");
            }
            
            urlSigner = UrlSigner.FromCredential(googleCredential);
            storageClient = StorageClient.Create(googleCredential);
            bucketName = Environment.GetEnvironmentVariable("BUCKET_NAME") ?? "box-files";
        }

        public async Task<string> UploadFileAsync(MemoryStream file, string fileNameForStorage)
        {
            var dataObject = await storageClient.UploadObjectAsync(bucketName, fileNameForStorage, null, file);
            return dataObject.Name;
        }

        public Task<string> GetFileAsync(string fileName)
        {
            // V4 is the default signing version.
            string url = urlSigner.Sign(bucketName, fileName, TimeSpan.FromHours(1), HttpMethod.Get);
            return Task.FromResult(url);
        }

        public async Task DeleteFileAsync(string fileNameForStorage)
        {
            await storageClient.DeleteObjectAsync(bucketName, fileNameForStorage);
        }

        public static Stream GenerateStreamFromString(string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }
}
