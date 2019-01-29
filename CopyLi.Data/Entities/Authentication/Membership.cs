using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CopyLi.Data.Entities.Authentication
{
    public class Membership : Entity<long>, ISoftDelete, IAuditable
    {
        #region [ Membership ]
        public string UserName { get; set; }
        public string Password { get; set; }
        public string PasswordSalt { get; set; }
        public bool IsActive { get; set; }
        #region [Role]
        public virtual Role Role { get; set; }
        public long? RoleId { get; set; }
        #endregion
        public virtual ICollection<ApplicationRefreshToken> RefreshTokens { get; set; }
        #endregion

        #region [ Deleting ]
        public DateTime? Deleted { get; set; }
        #endregion

        #region [ Audeting ]
        public long CreatedById { get; set; }
        public DateTime CreatedOn { get; set; }
        public long? UpdatedById { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public DateTime? LastLoginDate { get; set; }
        #endregion
    }
}
