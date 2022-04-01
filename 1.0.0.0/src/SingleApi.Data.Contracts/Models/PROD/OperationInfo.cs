namespace SingleApi.Data.Contracts.Models.PROD
{
    public class OperationInfo
    {
        public string MerchantName { get; set; }
        public string MerchantCode { get; set; }
        public string TransactionId { get; set; }
        public string BankOperationId { get; set; }
        public decimal Amount { get; set; }
        public string CurSymbol { get; set; }
        public DateTime ExpirationDateTime { get; set; }
    }
}
