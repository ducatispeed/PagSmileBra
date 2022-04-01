namespace SingleApi.Svc.PaySafe.Models.Requests
{
    public class CallBackPaymentRequest
    {
        public string OperationId { get; set; }
        public string CustomerIp { get; set; }
        public string ConsumerId { get; set; }
        public string CountryCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string Zip { get; set; }
        public string Country { get; set; }
    }
}
