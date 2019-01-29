using CopyLi.Data.Entities;
using Framework.Data.EF;
using System.Collections.Generic;

namespace Mimo.ControlPanel.Data.Specifications
{
    public class ById<TEntity, TKey> : Specification<TEntity>
      where TKey : struct
      where TEntity : Entity<long>
    {
        public ById(long id)
        {
            Expression = e => e.Id == id;
        }
        public ById(List<long> ids)
        {
            Expression = t => ids.Contains(t.Id);
        }
    }
}
