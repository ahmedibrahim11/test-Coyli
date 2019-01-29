using CopyLi.Data.Entities.Requests;
using Framework.Data.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CopyLi.Data.Specifications.RequestVendors
{
   public class ByRequestId: Specification<RequestVendor>
    {
        public ByRequestId(long reqId)
        {
            this.Expression = e => e.RequestId == reqId;
        }
    }
}
