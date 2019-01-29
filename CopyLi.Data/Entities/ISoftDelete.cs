using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CopyLi.Data.Entities
{
    public interface ISoftDelete
    {
        DateTime? Deleted { set; get; }
    }
}
