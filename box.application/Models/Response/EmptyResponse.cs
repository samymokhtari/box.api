namespace box.application.Models.Response
{
    public class EmptyResponse : UseCaseResponseMessage
    {
        public IEnumerable<Error> Errors { get; } = Enumerable.Empty<Error>();

        public EmptyResponse(IEnumerable<Error> errors, bool success = false, string message = "") : base(success, message)
        {
            Errors = errors;
        }

        public EmptyResponse(bool success = true, string message = "") : base(success, message)
        {
        }
    }
}