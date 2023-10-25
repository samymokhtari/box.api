using Microsoft.AspNetCore.StaticFiles;


namespace box.application.Models
{
    public class MyFile
    {
        public string Name { get; set; }
        public byte[] Content { get; set; }

        public MyFile(string name, byte[] content) 
        {
            Name = name;
            Content = content;
        }
        public string GetMimeTypeForFileExtension()
        {
            const string DefaultContentType = "application/octet-stream";

            var provider = new FileExtensionContentTypeProvider();

            if (!provider.TryGetContentType(this.Name, out string contentType))
            {
                contentType = DefaultContentType;
            }

            return contentType;
        }
    }

}



