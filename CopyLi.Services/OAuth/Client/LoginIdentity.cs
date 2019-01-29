using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CopyLi.Services.OAuth.Client
{
   public class LoginIdentity
    {
        public long Id { set; get; }
        public List<string> Role { set; get; }
        public string Name { set; get; }
    }
}
