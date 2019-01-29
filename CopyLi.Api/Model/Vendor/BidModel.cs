namespace CopyLi.Api.Model.Vendor
{
    public class BidModel
    {
        public long RequestId { get; set; }
        public string BidData { get; set; }
    }

    public class ReviewModel
    {
        public long OrderId { get; set; }
        public string ReviewData { get; set; }
    }
}