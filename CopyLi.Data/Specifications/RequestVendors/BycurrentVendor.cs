using CopyLi.Data.Entities.Requests;
using Framework.Data.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CopyLi.Data.Specifications.RequestVendors
{
   public class ByCurrentVendor:Specification<RequestVendor>
    {
        public ByCurrentVendor( long id)
        {
            this.Expression = e => e.VendorId == id && e.Request.RequestStatus == RequestStatus.pending;
        }
    }
}
