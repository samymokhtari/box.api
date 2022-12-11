using AutoMapper;
using box.application.Models;
using box.infrastructure.Data.Entities;

namespace box.application
{
    public class Profiles : Profile
    {
        public Profiles()
        {
            CreateMap<TFile, BoxFile>().ReverseMap();
            CreateMap<TProject, BoxFile>().ReverseMap();
        }
    }
}