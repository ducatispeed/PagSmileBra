namespace SingleApi.Svc.Contracts.Paysafe.Models.Requests
{
    public class PostPaymentHubHandleRequest
    {
        public string MerchantRefNum { get; set; }
        public decimal Amount { get; set; }
        public string CurrencyCode { get; set; }
        public string PaymentHandleToken { get; set; }
        public string Description { get; set; }
        public string CustomerIp { get; set; }
    }
}
