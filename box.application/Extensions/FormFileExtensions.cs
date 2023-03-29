using Microsoft.AspNetCore.Http;

namespace box.application.Extensions
{
    public static class FormFileExtensions
    {
        public static byte[] GetByteArray(this IFormFile file)
        {
            if (file.Length > 0)
            {
                using var ms = new MemoryStream();
                file.CopyTo(ms);
                return ms.ToArray();
            }
            else
            {
                throw new ArgumentException("File content is empty");
            }
        }
    }
}