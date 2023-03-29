using box.application.Interfaces;
using Microsoft.Extensions.Configuration;

namespace box.application.Models.Paths
{
    public class StorageRootPath : IStorageRootPath
    {
        public string RootPath { get; }

        public char DirectorySeparator => Path.DirectorySeparatorChar;

        public StorageRootPath(IConfiguration configuration)
        {
            RootPath = configuration["StorageRootPath"] ?? throw new ArgumentNullException("Storage Root Path can't be null");
        }
    }
}