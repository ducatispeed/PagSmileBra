using SingleApi.Infrastructure.Enums;

namespace SingleApi.Svc.Contracts.Paysafe.Models.Responses
{
    public class CreateBankPaymentHandleResponse
    {
        public string MerchantRefNum { get; set; }
        public double Amount { get; set; }
        public string CurrencyCode { get; set; }
        public string PaymentHandleToken { get; set; }
        public string Description { get; set; }
        public string CustomerIp { get; set; }
       
        public BaseResponce Error { get; set; }

        public class BaseResponce
        {
            public bool IsSuccess { get; set; }
            public string ErrorMessage { get; set; }
            public ErrorTypes ErrorCode { get; set; }
        }
    }
}
