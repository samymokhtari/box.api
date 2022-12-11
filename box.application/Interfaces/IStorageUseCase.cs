using box.application.Models.Request;
using box.application.Models.Response;

namespace box.application.Interfaces
{
    public interface IStorageUseCase : IUseCaseRequestHandler<StorageRequest, StorageResponse>
    {
    }
}