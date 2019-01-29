using CopyLi.Data.Entities.Orders;
using CopyLi.Data.Entities.Requests;
using CopyLi.Data.Entities.Service;

namespace CopyLi.Data.Entities.Items
{
    public class Item : Entity<long>
    {
        public string Name { get; set; }
        public string Descrption { get; set; }
        public string Data { get; set; }

        public long ServiceTypeId { get; set; }
        public ServiceType ServiceType { get; set; }

        #region [Request]
        public long RequestId { get; set; }
        public Request Request { get; set; }
        #endregion

    }
}
