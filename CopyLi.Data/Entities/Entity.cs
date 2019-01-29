using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CopyLi.Data.Entities
{
    public abstract class Entity<TKey>
    {
        public TKey Id { set; get; }
    }
}
