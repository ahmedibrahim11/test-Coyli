using AutoMapper;
using CopyLi.Data.Entities.Users.Customers;
using CopyLi.Utilites.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CopyLi.Api.Dto
{
    [Mapper]
    public class CustomerDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        //public ICollection<RequestDto> Requests { get; set; }

        public static void RegisterMapping(IMapperConfiguration Config)
        {
            Config.CreateMap<Customer, CustomerDto>();
        }

    }
}