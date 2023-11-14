using box.application.Interfaces;
using Microsoft.Extensions.Configuration;

namespace box.application.Models.Paths
{
    internal class StorageRootPath : IStorageRootPath
    {
        public string RootPath { get; set; }

        public static char DirectorySeparator => Path.AltDirectorySeparatorChar;

        private const string DefaultFolderName = $"box";

        private static string GetDefaultRootPath()
        {
            Console.WriteLine($"{Path.GetPathRoot(Environment.SystemDirectory)}{DirectorySeparator}{DefaultFolderName}");
            return $"{Path.GetPathRoot(Environment.SystemDirectory)}{DirectorySeparator}{DefaultFolderName}";
        }

        public StorageRootPath(IConfiguration configuration)
        {
            RootPath = configuration["StorageRootPath"] ?? GetDefaultRootPath();
        }
    }
}