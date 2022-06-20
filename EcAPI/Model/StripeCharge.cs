namespace EcAPI.Model
{
    public class StripeCharge
    {
        public long Amount { get; set; }
        public string Currency { get; set; }
        public string Source { get; set; }
        public string ReceiptEmail { get; set; }
        public string ProductName { get; set; }

    }
}
