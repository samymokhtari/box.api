using box.application.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace box.application.Models.Paths
{
    internal class StorageRootPath : IStorageRootPath
    {
        public string RootPath { get; set; }

        public StorageRootPath(IConfiguration configuration)
        {
            RootPath = configuration["StorageRootPath"];
        }
    }
}
