namespace box.application.Models.Response
{
    public class StorageGetResponse : UseCaseResponseMessage
    {
        public ByteArrayContent FileStream { get; }
        public IEnumerable<Error> Errors { get; } = Enumerable.Empty<Error>();

        public StorageGetResponse(IEnumerable<Error> errors, bool success = false, string message = "") : base(success, message)
        {
            Errors = errors;
        }

        public StorageGetResponse(ByteArrayContent file, bool success = true, string message = "") : base(success, message)
        {
            FileStream = file;
        }
    }
}