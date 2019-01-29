using System.Collections.Generic;

namespace CopyLi.Api.Model.Request
{
    public class RequestModel
    {
        public int? Id { get; set; }
        public int CustomerId { get; set; }
        public List<RequestItemModel> Items { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }

    public class RequestItemModel
    {
        public string Data { get; set; }
        public int ServiceTypeId { get; set; }
    }
}