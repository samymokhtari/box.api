using AutoMapper;
using box.application.Models;
using box.application.Persistance;
using box.infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace box.infrastructure.Data.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly BoxContext _boxContext;
        private readonly IMapper _mapper;

        public ProjectRepository(BoxContext boxContext, IMapper mapper)
        {
            _boxContext = boxContext;
            _mapper = mapper;
        }

        Task IProjectRepository.AddAsync(BoxProject project)
        {
            _boxContext.TProjects.Add(_mapper.Map<TProject>(project));
            return _boxContext.SaveChangesAsync();
        }

        BoxProject IProjectRepository.GetByCode(string code)
        {
            return _mapper.Map<BoxProject>(_boxContext.TProjects.SingleOrDefault(x => x.Code == code));
        }

        BoxProject IProjectRepository.GetById(int id)
        {
            return _mapper.Map<BoxProject>(_boxContext.TProjects.SingleOrDefault(x => x.Id == id));
        }

        IAsyncEnumerable<BoxProject> IProjectRepository.ListProjects(BoxProject project)
        {
            return _boxContext.TProjects.Select(
                x => _mapper.Map<BoxProject>(x)).AsAsyncEnumerable();
        }
    }
}