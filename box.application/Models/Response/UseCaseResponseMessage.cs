namespace box.application.Models.Response
{
    public abstract class UseCaseResponseMessage
    {
        public bool Success { get; }
        public string Message { get; }

        protected UseCaseResponseMessage(bool success = false, string message = "")
        {
            Success = success;
            Message = message;
        }
    }
}
