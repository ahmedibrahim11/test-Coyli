using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CopyLi.Data.Entities
{
    public interface IAuditable
    {
        long CreatedById { set; get; }
        DateTime CreatedOn { set; get; }

        long? UpdatedById { set; get; }
        DateTime? UpdatedOn { set; get; }
    }
}
