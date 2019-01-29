using AutoMapper;
using CopyLi.Data.Entities.Orders;
using CopyLi.Utilites.AutoMapper;
using System;
using System.Collections.Generic;

namespace CopyLi.Api.Dto
{
    [Mapper]
    public class OrderDto
    {
        public long Id { get; set; }
        public long RequestId { get; set; }
        public string CustomerName { get; set; }
        public string LocationName { get; set; }
        public DateTime CreatedOn { get; set; }

        public ICollection<ItemOrderDto> Items { get; set; }
        public string ReviewData { get; set; }

        public static void RegisterMapping(IMapperConfiguration Config)
        {
            Config.CreateMap<Order, OrderDto>()
                  .ForMember(dist => dist.CustomerName, x => x.MapFrom(src => src.Customer.Name))
                  .ForMember(dist => dist.LocationName, x => x.MapFrom(src => src.Customer.LocationName));
        }
    }
}