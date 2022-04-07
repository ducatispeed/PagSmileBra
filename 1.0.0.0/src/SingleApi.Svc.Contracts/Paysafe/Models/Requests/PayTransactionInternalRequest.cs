using SingleApi.Infrastructure.Enums;

namespace SingleApi.Svc.Contracts.Paysafe.Models.Requests
{
    public class PayInternalTransactionRequest
    {
        public string BankId { get; set; }
        public string BankReference { get; set; }
        public Channel ChannelId { get; set; }
        public string OperationId { get; set; }
        public string ShopperAmount { get; set; }
        public string ShopperCurrencyId { get; set; }
        public bool IsForced { get; set; }
    }
}
