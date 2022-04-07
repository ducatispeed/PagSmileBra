namespace SingleApi.Svc.Contracts.Paysafe.Models.Responses
{
    public class CallbackPaymentHandleResponse
    {
        public string Id { get; set; }
        public decimal? Amount { get; set; }
        public string CurrencyCode { get; set; }
        public string MerchantRefNum { get; set; }
        public string Action { get; set; }
        public string Usage { get; set; }
        public string Status { get; set; }
        public string PaymentType { get; set; }
        public string ExecutionMode { get; set; }
        public string CustomerIp { get; set; }
        public string PaymentHandleToken { get; set; }
        public BillingDetailsModel BillingDetails { get; set; }
        public ProfileModel Profile { get; set; }
        public RapidTransferModel RapidTransfer { get; set; }
        public string GatewayReconciliationId { get; set; }
        public GatewayResponseModel GatewayResponse { get; set; }
        public IEnumerable<ReturnLinkModel> ReturnLinks { get; set; }
        public IEnumerable<ReturnLinkModel> Links { get; set; }
        public int? TimeToLiveSeconds { get; set; }
        public string TransactionType { get; set; }
        public ErrorModel Error { get; set; }


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
            public string Sid { get; set; }
            public string Processor { get; set; }
        }

        public class ReturnLinkModel
        {
            public string Rel { get; set; }
            public string Href { get; set; }
            public string Method { get; set; }
        }

        public class ErrorModel
        {
            public string Message { get; set; }
            public string Code { get; set; }
        }
    }

}
