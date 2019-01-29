using CopyLi.Data.Entities.Users.Vendors;

namespace CopyLi.Data.Entities.Requests
{
    public class RequestVendor : Entity<long>
    {
        #region [Request]
        public long RequestId { get; set; }
        public virtual Request Request { get; set; }
        #endregion
        public long VendorId { get; set; }
        public virtual Vendor Vendor { get; set; }
    }
}
