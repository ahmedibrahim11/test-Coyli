using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CopyLi.Api.Model
{
    public class ChangePasswordModel
    {
        public string  OldPassword { get; set; }
        public string  NewPassword { get; set; }
    }
}