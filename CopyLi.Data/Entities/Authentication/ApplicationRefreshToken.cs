using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CopyLi.Data.Entities.Authentication
{
    public class ApplicationRefreshToken : Entity<int>
    {
        /// <summary>
        /// Gets or sets the refresh token.
        /// </summary>
        /// <value>
        /// The refresh token.
        /// </value>
        public string RefreshToken { get; set; }
        /// <summary>
        /// Gets or sets the application identifier.
        /// </summary>
        /// <value>
        /// The application identifier.
        /// </value>
        public int ApplicationId { get; set; }
        public Application Application { get; set; }
        public long MembershipId { get; set; }
        public Membership Membership { get; set; }
        public DateTime IssuedUtc { get; set; }
        public DateTime ExpiresOnUtc { get; set; }
    }

}
