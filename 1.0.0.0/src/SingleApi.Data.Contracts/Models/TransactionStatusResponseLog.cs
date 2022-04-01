namespace SingleApi.Data.Contracts.Models
{
    public class TransactionStatusResponseLog
    {
        public Guid TokenId { get; set; }
        public string OperationId { get; set; }
        public string Description { get; set; }
        public string ErrorCode { get; set; }
        public string BankID { get; set; }
    }
}
