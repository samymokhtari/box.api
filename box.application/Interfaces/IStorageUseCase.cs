using box.application.Models.Request;

namespace box.application.Interfaces
{
    public interface IStorageUseCase
    {
        Task<bool> HandleAsync(StorageRequest request);
    }
}