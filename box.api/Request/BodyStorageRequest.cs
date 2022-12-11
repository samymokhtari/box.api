using System.ComponentModel.DataAnnotations;

namespace box.api.Request
{
    public class BodyStorageRequest
    {
            /// <summary>
            /// File
            /// </summary>
            [Required]
            public IFormFile File { get; set; }
    }
}
