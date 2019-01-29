using CopyLi.Data.Entities.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CopyLi.Api.Model.Vendor
{
    public class CreateVendorModel
    {
        #region [Properties]
        public string Name { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Password { get; set; }
        #endregion
    }
}