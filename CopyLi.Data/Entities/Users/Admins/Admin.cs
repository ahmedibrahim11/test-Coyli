using CopyLi.Data.Entities.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CopyLi.Data.Entities.Users.Admins
{
   public class Admin:Entity<long>
    {
        public string Name { get; set; }
        public string Email { get; set; }

        #region [ Membership ]
        public virtual Membership Membership { get; set; }
        public long MembershipId { get; set; }

        #endregion
    }
}
