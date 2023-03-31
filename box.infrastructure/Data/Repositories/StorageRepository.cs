using AutoMapper;
using box.application.Models;
using box.application.Persistance;
using box.infrastructure.Data.Entities;

namespace box.infrastructure.Data.Repositories
{
    public class StorageRepository : IStorageRepository
    {
        private readonly BoxContext _boxContext;
        private readonly IMapper _mapper;

        public StorageRepository(BoxContext boxContext, IMapper mapper)
        {
            _boxContext = boxContext;
            _mapper = mapper;
        }

        public Task AddAsync(BoxFile file)
        {
            _boxContext.TFiles.Add(_mapper.Map<TFile>(file));
            return _boxContext.SaveChangesAsync();
        }

        public BoxFile GetById(int id)
        {
            return _mapper.Map<BoxFile>(_boxContext.TFiles.SingleOrDefault(x => x.Id == id));
        }

        public BoxFile GetByName(string name, int projectId)
        {
            return _mapper.Map<BoxFile>(_boxContext.TFiles.SingleOrDefault(x => x.Filename == name && x.ProjectId == projectId));
        }
    }
}