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
    public class RequestHistoryDto
    {
        public long Id { get; set; }
        public long CreatedById { get; set; }
        public string CustomerName { get; set; }
        public DateTime CreatedOn { get; set; }
        public long? UpdatedById { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public long RequestId { get; set; }
        public long? vendorId { get; set; }
        public RequestStatus RequestStatus { get; set; }
        public static void RegisterMapping(IMapperConfiguration Config)
        {
            Config.CreateMap<RequestHistory, RequestHistoryDto>()
            .ForMember(dist => dist.CustomerName, x => x.MapFrom(src => src.Customer.Name));
        }
    }
}