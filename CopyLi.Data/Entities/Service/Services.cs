using System.Collections.Generic;

namespace CopyLi.Data.Entities.Service
{
    public class Services : Entity<long>
    {
        public string Title { get; set; }

        public string TitleAr { get; set; }
        public int? ParentId { get; set; }

        #region [Types]
        public virtual ICollection<ServiceType> ServiceTypes { get; set; }
        #endregion
       

    }
}
