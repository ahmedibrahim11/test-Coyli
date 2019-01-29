using CopyLi.Data.Entities.Users.Customers;
using CopyLi.Data.Entities.Users.Vendors;
using System;
using System.Collections.Generic;

namespace CopyLi.Data.Entities.Orders
{
    public class Order : Entity<long>, ISoftDelete, IAuditable
    {

        #region [Customer]
        public Customer Customer { get; set; }
        public long CustomerId { get; set; }
        #endregion
        public string BidData { get; set; }
        public bool CoverLetter { get; set; }
        public bool HoldingPlasticFile { get; set; }
        public OrderStatus Status { get; set; }
        public string ReviewData { get; set; }
        #region [Vendor]
        public Vendor Vendor { get; set; }
        public long VendorId { get; set; }
        #endregion

        #region [Request]
        public long RequestId { get; set; }
        #endregion

        #region [Items]
        public virtual ICollection<ItemOrder> Items { get; set; }
        #endregion
        public double Latitude { get; set; }
        public double Longitude { get; set; }
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
}
