using CopyLi.Data.Entities.Users.Admins;
using Framework.Data.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CopyLi.Data.Specifications.Admins
{
  public class ByMemberShipId:Specification<Admin>
    {
        public ByMemberShipId(long membershipId)
        {
            this.Expression = e => e.MembershipId == membershipId;
        }
    }
}
