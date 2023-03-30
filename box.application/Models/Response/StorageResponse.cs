namespace box.application.Models.Response
{
    public class StorageResponse : UseCaseResponseMessage
    {
        public string FileName { get; }
        public IEnumerable<Error> Errors { get; } = Enumerable.Empty<Error>();

        public StorageResponse(IEnumerable<Error> errors, bool success = false, string message = "") : base(success, message)
        {
            Errors = errors;
            FileName = string.Empty;
        }

        public StorageResponse(string filename, bool success = true, string message = "") : base(success, message)
        {
            FileName = filename;
        }
    }
}