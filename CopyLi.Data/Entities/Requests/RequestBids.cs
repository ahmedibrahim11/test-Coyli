using CopyLi.Data.Entities.Users.Vendors;

namespace CopyLi.Data.Entities.Requests
{
    public class RequestBid : Entity<long>
    {
        public decimal Price { get; set; }
        public string Data { get; set; }

        #region [Request]
        public long RequestId { get; set; }
        public virtual Request Request { get; set; }
        #endregion
  


    }
}
