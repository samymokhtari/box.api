using box.application.Interfaces;
using box.application.Models.Response;

namespace box.api.Presenters
{
    public sealed class StorageGetPresenter : IOutputPort<StorageGetResponse>
    {
        public Stream ContentResult { get; set; }

        public StorageGetPresenter()
        {
            ContentResult = new MemoryStream();
        }

        public void Handle(StorageGetResponse response)
        {
            ContentResult = response.FileStream.ReadAsStream();
        }
    }
}