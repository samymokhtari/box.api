using box.api.Serialization;
using box.application.Interfaces;
using box.application.Models.Response;
using System.Net;

namespace box.api.Presenters
{
    public class ProjectPresenter : IOutputPort<ProjectResponse>
    {
        public JsonContentResult ContentResult { get; }

        public ProjectPresenter()
        {
            ContentResult = new JsonContentResult();
        }

        public void Handle(ProjectResponse response)
        {
            ContentResult.StatusCode = (int)(response.Success ? HttpStatusCode.OK : HttpStatusCode.BadRequest);
            ContentResult.Content = JsonSerializer.SerializeObject(response);
        }
    }
}
