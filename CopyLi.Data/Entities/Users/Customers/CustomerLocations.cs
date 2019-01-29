using System;

namespace CopyLi.Data.Entities.Users.Customers
{
    public class CustomerLocations : Entity<long>, ISoftDelete
    {
        public string Title { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public DateTime? Deleted { get; set; }
        public long CustomerId { get; set; }
    }
}
