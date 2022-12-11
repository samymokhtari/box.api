using box.api.Serialization;
using box.application.Interfaces;
using box.application.Models.Response;
using System.Net;

namespace box.api.Presenters
{
    public sealed class StoragePresenter : IOutputPort<StorageResponse>
    {
        public JsonContentResult ContentResult { get; }

        public StoragePresenter()
        {
            ContentResult = new JsonContentResult();
        }

        public void Handle(StorageResponse response)
        {
            ContentResult.StatusCode = (int)(response.Success ? HttpStatusCode.OK : HttpStatusCode.BadRequest);
            ContentResult.Content = JsonSerializer.SerializeObject(response);
        }
    }
}