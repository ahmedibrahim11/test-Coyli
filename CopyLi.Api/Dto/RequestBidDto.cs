using AutoMapper;
using CopyLi.Data.Entities.Requests;
using CopyLi.Utilites.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CopyLi.Api.Dto
{
    [Mapper]
    public class RequestBidDto
    {
        public long Id { get; set; }
        public string Data { get; set; }
        public string VendorName { get; set; }
        public static void RegisterMapping(IMapperConfiguration Config)
        {
            Config.CreateMap<RequestBid, RequestBidDto>()
               .ForMember(dist => dist.VendorName,
               reqBid => reqBid.MapFrom(src => src.Request.Vendor.Name));
        }

    }
}