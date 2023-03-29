using box.application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace box.application.Persistance
{
    public interface IProjectRepository : IRepository
    {
        Task AddAsync(BoxProject project);
        BoxProject GetById(int id);
        BoxProject GetByCode(string code);
        IAsyncEnumerable<BoxProject> ListProjects(BoxProject project);

    }
}
