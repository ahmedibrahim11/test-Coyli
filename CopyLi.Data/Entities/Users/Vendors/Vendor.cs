using CopyLi.Data.Entities.Authentication;
using CopyLi.Data.Entities.Requests;
using System.Collections.Generic;

namespace CopyLi.Data.Entities.Users.Vendors
{
    public class Vendor : Entity<long>
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }

        #region [ Membership ]
        public virtual Membership Membership { get; set; }
        public long MembershipId { get; set; }

        #endregion

        #region Location
        public string Location { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        #endregion

        #region [Request]
        public virtual ICollection<Request> Requests { get; set; }
        #endregion
        public string Token { get; set; }
    }
}
