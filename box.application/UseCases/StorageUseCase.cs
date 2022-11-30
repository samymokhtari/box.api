using box.application.Interfaces;
using box.application.Models.Request;
using Microsoft.Extensions.Configuration;

namespace box.application.UseCases
{
    public class StorageUseCase : AUseCase, IStorageUseCase
    {
        public StorageUseCase(IConfiguration configuration) : base(configuration)
        {
        }

        Task<bool> IStorageUseCase.HandleAsync(StorageRequest request)
        {
            throw new NotImplementedException();
        }
    }
}