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

        public char DirectorySeparator => Path.DirectorySeparatorChar;

        private const string DefaultPathWindows = "c:/box/";
        private const string DefaultPathLinux = "/box/";

        private static string GetDefaultRootPath()
        {
            if(RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return DefaultPathWindows;
            }   
            else
            {
                return DefaultPathLinux;
            }
        }

        public StorageRootPath(IConfiguration configuration)
        {
            RootPath = configuration["StorageRootPath"] ?? GetDefaultRootPath();
        }
    }
}