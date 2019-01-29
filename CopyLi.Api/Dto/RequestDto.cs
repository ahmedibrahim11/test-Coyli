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
    public class RequestDto
    {
        public long Id { get; set; }
        public RequestStatus RequestStatus { get; set; }
        public string CustomerName { get; set; }
        public string LocationName { get; set; }
        public DateTime CreatedOn { get; set; }

        public ICollection<ItemDto> Items { get; set; }
        public string CustomerToken { get; set; }
        public static void RegisterMapping(IMapperConfiguration Config)
        {
            Config.CreateMap<Request, RequestDto>()
                  .ForMember(dist => dist.CustomerName, x => x.MapFrom(src => src.Customer.Name))
                  .ForMember(dist => dist.LocationName, x => x.MapFrom(src => src.Customer.LocationName))
                  .ForMember(dist => dist.CustomerToken, x => x.MapFrom(src => src.Customer.Token));
        }
    }
}