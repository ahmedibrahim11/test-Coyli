using AutoMapper;
using CopyLi.Data.Entities.Service;
using CopyLi.Utilites.AutoMapper;
using System.Collections.Generic;

namespace CopyLi.Api.Dto
{
    public class ServicesDto
    {
        public IEnumerable<ServiceDto> Services { get; set; }
        public IEnumerable<ServiceTypeDto> ServiceTypes { get; set; }

    }

    [Mapper]
    public class ServiceDto
    {
        public string Title { get; set; }

        public string TitleAr { get; set; }
        public int? ParentId { get; set; }
        public static void RegisterMapping(IMapperConfiguration Config)
        {
            Config.CreateMap<Data.Entities.Service.Services, ServiceDto>();
        }

    }

    [Mapper]
    public class ServiceTypeDto
    {
        public string Title { get; set; }
        public string TitleAr { get; set; }
        public string Properties { get; set; }
        public static void RegisterMapping(IMapperConfiguration Config)
        {
            Config.CreateMap<ServiceType, ServiceTypeDto>();
        }
    }
}
