using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CopyLi.Data.Entities.Authentication
{
    public class Role : Entity<long>
    {
        public string Name { get; set; }
    }
}
