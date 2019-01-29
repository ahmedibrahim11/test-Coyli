namespace CopyLi.Api.Model.Vendor
{
    public class AddEditVendorLocationModel
    {
        public long VendorId { get; set; }
        public string Location { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}