using CopyLi.Data.Entities.Authentication;
using CopyLi.Data.Entities.Requests;
using System.Collections.Generic;

namespace CopyLi.Data.Entities.Users.Customers
{
    public class Customer : Entity<long>
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }

        #region [ Membership ]
        public virtual Membership Membership { get; set; }
        public long MembershipId { get; set; }

        #endregion

        #region Locations
        public decimal Latitude { get; set; }
        public string  LocationName { get; set; }
        public decimal Longitude { get; set; }
        //public ICollection<CustomerLocations> CustomerLocations { get; set; }
        #endregion

        public ICollection<Request> Requests { get; set; }
        public ICollection<RequestHistory> RequestHistorires { get; set; }

        public string Token { get; set; }

    }
}
