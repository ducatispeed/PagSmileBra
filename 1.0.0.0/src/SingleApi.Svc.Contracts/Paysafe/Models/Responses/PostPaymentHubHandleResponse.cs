
namespace SingleApi.Svc.Contracts.Paysafe.Models.Responses
{
    public class PostPaymentHubHandleResponse
    {
        public string Id { get; set; }
        public string MerchantRefNum { get; set; }
        public decimal? Amount { get; set; }
        public string CurrencyCode { get; set; }
        public string PaymentHandleToken { get; set; }
        public string PaymentType { get; set; }
        public ProfileModel Profile { get; set; }
        public RapidTransferModel RapidTransfer { get; set; }
        public string Decription { get; set; }
        public string CustomerIp { get; set; }
        public decimal AvailableToSettle { get; set; }
        public string Status { get; set; }
        public DateTime TxnTime { get; set; }
        public Guid GatewayReconciliationId { get; set; }
        public GatewayResponseModel GatewayResponse { get; set; }

        public class BillingDetailsModel
        {
            public string Street { get; set; }
            public string City { get; set; }
            public string Zip { get; set; }
            public string Country { get; set; }
        }

        public class ProfileModel
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }

        public class RapidTransferModel
        {
            public string ConsumerId { get; set; }
            public string CountryCode { get; set; }
        }

        public class GatewayResponseModel
        {
            public int Id { get; set; }
            public string Sid { get; set; }
            public string Processor { get; set; }
        }
    }
}
