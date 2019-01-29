using CopyLi.Data.Entities.Users.Vendors;
using Framework.Data.EF;

namespace CopyLi.Data.Specifications.Vendors
{
    public class ByMembershipId : Specification<Vendor>
    {
        public ByMembershipId(long membershipId)
        {
            this.Expression = e => e.MembershipId == membershipId;
        }

    }
}
