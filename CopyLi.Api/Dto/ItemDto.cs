using AutoMapper;
using CopyLi.Data.Entities.Items;
using CopyLi.Utilites.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CopyLi.Api.Dto
{
    [Mapper]
    public class ItemDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Descrption { get; set; }
        public string Data { get; set; }
        public ServiceTypeDto ServiceType { get; set; }

        public static void RegisterMapping(IMapperConfiguration Config)
        {
            Config.CreateMap<Item, ItemDto>();
        }
    }
}