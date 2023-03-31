using box.application.Models;

namespace box.application.Persistance
{
    public interface IStorageRepository
    {
        Task AddAsync(BoxFile file);

        BoxFile GetById(int id);

        BoxFile GetByName(string name, int projectId);
    }
}