namespace box.application.Models.Response
{
    public class StorageResponse : UseCaseResponseMessage
    {
        public Guid ObjectID { get; }
        public IEnumerable<Error> Errors { get; } = Enumerable.Empty<Error>();

        public StorageResponse(IEnumerable<Error> errors, bool success = false, string message = "") : base(success, message)
        {
            Errors = errors;
        }

        public StorageResponse(Guid objectId, bool success = false, string message = "") : base(success, message)
        {
            ObjectID = objectId;
        }
    }
}