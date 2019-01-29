using CopyLi.Data.Entities.Authentication;
using CopyLi.Data.Entities.Users.Admins;
using CopyLi.Data.Entities.Users.Customers;
using CopyLi.Data.Entities.Users.Vendors;
using Framework.Security.OAuth;
using System;
using System.Collections.Generic;

namespace CopyLi.Services.OAuth.Server
{
    public class MembershipDto : IUser
    {
        #region Constructors

        public MembershipDto()
        {
            this.AdditionalResponseParameters = new Dictionary<string, string>();
            this.AdditionalTokenParameters = new Dictionary<string, string>();
        }

        #endregion Constructors

        #region Properties

        public Dictionary<string, string> AdditionalResponseParameters
        {
            set; get;
        }

        public Dictionary<string, string> AdditionalTokenParameters
        {
            get;
            set;
        }

        public string Id
        {
            get; set;
        }

        public string[] Roles
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the token issue date.
        /// </summary>
        /// <remarks>
        /// used only to set the refresh token expiry date.
        /// </remarks>
        /// <value>
        /// The token issue date.
        /// </value>
        public DateTime TokenIssueDate
        {
            get; set;
        }

        public string Username
        {
            get; set;
        }

        #endregion Properties

        #region Methods

        public void SetAddionalResponceParams(Membership dbUser)
        {
            this.AdditionalTokenParameters.Add("Name", dbUser.UserName);
            this.AdditionalResponseParameters = new Dictionary<string, string>()
                {
                    { "id", dbUser.Id.ToString() },
                    { "name", dbUser.UserName },
                   //{ "role",dbUser.Role.Name}
                    {"role", "Admin"},
                };
        }

        public void SetAddionalResponceParams(Vendor dbVendor)
        {
            this.AdditionalTokenParameters.Add("vendorId", dbVendor.Id.ToString());

            this.AdditionalResponseParameters.Add("vendorId", dbVendor.Id.ToString());
            this.AdditionalResponseParameters.Add("vendorName", dbVendor.Name);
            this.AdditionalResponseParameters.Add("vendorEmail", dbVendor.Email);

        }

        public void SetAddionalResponceParams(Admin dbAdmin)
        {
            this.AdditionalTokenParameters.Add("adminId", dbAdmin.Id.ToString());
            this.AdditionalResponseParameters.Add("adminId", dbAdmin.Id.ToString());
            this.AdditionalResponseParameters.Add("adminName", dbAdmin.Name);
            this.AdditionalResponseParameters.Add("adminEmail", dbAdmin.Email);

        }
        public void SetAddionalResponceParams(Customer dbCustomer)
        {
            this.AdditionalTokenParameters.Add("customerId", dbCustomer.Id.ToString());

            this.AdditionalResponseParameters.Add("customerId", dbCustomer.Id.ToString());
            this.AdditionalResponseParameters.Add("customerName", dbCustomer.Name);
            this.AdditionalResponseParameters.Add("customerEmail", dbCustomer.Email);
        }

        #endregion Methods
    }
}
