using box.api.Serialization;
using box.application.Interfaces;
using box.application.Models.Response;
using System.Net;

namespace box.api.Presenters
{
    public class EmptyPresenter : IOutputPort<EmptyResponse>
    {
        public JsonContentResult ContentResult { get; }

        public EmptyPresenter()
        {
            ContentResult = new JsonContentResult();
        }

        public void Handle(EmptyResponse response)
        {
            ContentResult.StatusCode = (int)(response.Success ? HttpStatusCode.OK : HttpStatusCode.BadRequest);
            ContentResult.Content = JsonSerializer.SerializeObject(response);
        }
    }
}