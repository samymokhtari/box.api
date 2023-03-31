using box.application.Models;

namespace box.application.Persistance
{
    public interface IProjectRepository
    {
        Task AddAsync(BoxProject project);
        BoxProject GetById(int id);

        BoxProject GetByCode(string code);

        IAsyncEnumerable<BoxProject> ListProjects(BoxProject project);
    }
}