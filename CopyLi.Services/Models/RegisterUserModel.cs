using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CopyLi.Services.Models
{
    public class RegisterUserModel
    {
        public string FullName { set; get; }
        public string Mobile { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }

    }
}
