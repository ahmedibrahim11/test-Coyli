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
    public class RequestVendorDto
    {
        public RequestDto Request { get; set; }
        public VendorDto Vendor { get; set; }

        public static void RegisterMapping(IMapperConfiguration Config)
        {
            Config.CreateMap<RequestVendor, RequestVendorDto>();
        }
    }
}