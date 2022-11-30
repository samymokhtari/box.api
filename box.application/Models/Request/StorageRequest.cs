using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace box.application.Models.Request
{
    public class StorageRequest
    {
        /// <summary>
        /// File
        /// </summary>
        [Required]
        public IFormFile File { get; set; }
    }
}