
using SingleApi.Infrastructure.Enums;

namespace SingleApi.Svc.Contracts.Paysafe.Models.View
{
    public class CreateBankPaymentHandleViewModel
    {
        public string MerchantRefNum { get; set; }
        public decimal Amount { get; set; }
        public string CurrencyCode { get; set; }
        public string PaymentHandleToken { get; set; }
        public string Description { get; set; }
        public string CustomerIp { get; set; }

        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
        public ErrorTypes ErrorCode { get; set; }
    }
}
