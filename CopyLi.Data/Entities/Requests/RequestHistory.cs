using CopyLi.Data.Entities.Users.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CopyLi.Data.Entities.Requests
{
   public class RequestHistory:Entity<long>,IAuditable
    {
        #region [Customer]
        public Customer Customer { get; set; }
        public long CustomerId { get; set; }
        #endregion
        public long CreatedById { get; set; }
        public DateTime CreatedOn { get; set; }
        public long? UpdatedById { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public long RequestId { get; set; }
        //public virtual Request Request { get; set; }
        public long? vendorId { get; set; }
        public RequestStatus RequestStatus { get; set; }
    }
}
