using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CopyLi.Data.Entities.Orders
{
   public class OrderHistory: Entity<long>, IAuditable
    {
        public long CreatedById { get; set; }
        public DateTime CreatedOn { get; set; }
        public long? UpdatedById { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public long OrderId { get; set; }
        //public virtual Order Order { get; set; }
        public long? vendorId { get; set; }
        public OrderStatus OrderStatus { get; set; }
    }
  
}
