using AutoMapper;
using CopyLi.Data.Entities.Users.Vendors;
using CopyLi.Utilites.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CopyLi.Api.Dto
{
    [Mapper]
    public class VendorDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }

        public static void RegisterMapping(IMapperConfiguration Config)
        {
            Config.CreateMap<Vendor, VendorDto>()
          .ForMember(dist => dist.Id, x => x.MapFrom(src => src.Id));

        }
    }
}