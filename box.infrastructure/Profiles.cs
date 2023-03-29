using AutoMapper;
using box.application.Models;
using box.infrastructure.Data.Entities;

namespace box.infrastructure
{
    public class Profiles : Profile
    {
        public Profiles()
        {
            CreateMap<TFile, BoxFile>().ReverseMap();
            CreateMap<TProject, BoxProject>().ReverseMap();
        }
    }
}