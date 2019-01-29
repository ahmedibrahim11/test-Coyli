using CopyLi.Data.Entities.Items;
using CopyLi.Data.Entities.Orders;
using CopyLi.Data.Entities.Users.Customers;
using CopyLi.Data.Entities.Users.Vendors;
using System;
using System.Collections.Generic;

namespace CopyLi.Data.Entities.Requests
{
    public class Request:Entity<long>, ISoftDelete, IAuditable
    {
        #region [Items]
        public virtual ICollection<Item> Items { get; set; }
        #endregion
        public Customer Customer { get; set; }
        public long CustomerId { get; set; }
 
        #region [Vendor]
        public long VendorId { get; set; }
        public virtual Vendor Vendor { get; set; } 
        #endregion
        public RequestStatus RequestStatus{ get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public virtual RequestBid RequestBid { get; set; }

        #region [ Deleting ]
        public DateTime? Deleted { get; set; }
        #endregion

        #region [ Audeting ]
        public long CreatedById { get; set; }
        public DateTime CreatedOn { get; set; }
        public long? UpdatedById { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public DateTime? LastLoginDate { get; set; }
        #endregion
    }


    public enum RequestStatus
    {
        pending,order
    }
}
