using box.application.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace box.application.Models.Paths
{
    internal class StorageRootPath : IStorageRootPath
    {
        public string RootPath { get; set; }

        public static char DirectorySeparator => Path.DirectorySeparatorChar;

        private const string DefaultFolderName = $"box";

        private static string GetDefaultRootPath()
        {
                return $"{Path.GetPathRoot(Environment.SystemDirectory)}{DirectorySeparator}{DefaultFolderName}";
        }

        public StorageRootPath(IConfiguration configuration)
        {
            RootPath = configuration["StorageRootPath"] ?? GetDefaultRootPath();
        }
    }
}