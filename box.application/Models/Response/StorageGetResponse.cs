namespace box.application.Models.Response
{
    public class StorageGetResponse : UseCaseResponseMessage
    {
        public MyFile FileStream { get; }
        public IEnumerable<Error> Errors { get; } = Enumerable.Empty<Error>();

        public StorageGetResponse(IEnumerable<Error> errors, bool success = false, string message = "") : base(success, message)
        {
            Errors = errors;
            FileStream = new("", Array.Empty<byte>());
        }

        public StorageGetResponse(MyFile file, bool success = true, string message = "") : base(success, message)
        {
            FileStream = file;
        }
    }
}