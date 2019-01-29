using CopyLi.Data.Entities.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CopyLi.Data.Entities.Orders
{
  public class ItemOrder:Entity<long>
    {
        public string Name { get; set; }
        public string Descrption { get; set; }
        public string Data { get; set; }
        public long ServiceTypeId { get; set; }
        public ServiceType ServiceType { get; set; }

        #region [Order]
        public virtual Order Order { get; set; }
        public long  orderId { get; set; }
        #endregion

    }
}
