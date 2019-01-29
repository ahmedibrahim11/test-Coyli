using CopyLi.Data.Entities.Items;
using System.Collections.Generic;

namespace CopyLi.Data.Entities.Service
{
    public class ServiceType : Entity<long>
    {
        public string Title { get; set; }
        public string TitleAr { get; set; }
        public string Properties { get; set; }
        public string BidProperties { get; set; }
        public virtual ICollection<Item> Items { get; set; }
    }
}
