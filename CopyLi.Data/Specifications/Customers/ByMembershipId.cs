using CopyLi.Data.Entities.Users.Customers;
using Framework.Data.EF;

namespace CopyLi.Data.Specifications.Customers
{
    public class ByMembershipId : Specification<Customer>
    {
        public ByMembershipId(long membershipId)
        {
            this.Expression = e => e.MembershipId == membershipId;
        }

    }
}
