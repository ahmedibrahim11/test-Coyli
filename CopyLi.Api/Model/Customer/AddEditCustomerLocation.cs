namespace CopyLi.Api.Model.Customer
{
    public class AddEditCustomerLocation
    {
        public long Id { get; set; }    
        public long CustomerId { get; set; }
        public string Title { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
    }
}