namespace box.application.Models.Response
{
    public class ProjectResponse : UseCaseResponseMessage
    {
        public BoxProject? Project { get; set; }
        public IEnumerable<Error> Errors { get; } = Enumerable.Empty<Error>();

        public ProjectResponse(IEnumerable<Error> errors, bool success = false, string message = "") : base(success, message)
        {
            Errors = errors;
        }

        public ProjectResponse(BoxProject project, bool success = true, string message = "") : base(success, message)
        {
            Project = project;
        }
    }
}