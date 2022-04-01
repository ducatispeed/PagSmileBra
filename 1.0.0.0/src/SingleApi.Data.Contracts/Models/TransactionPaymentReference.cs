namespace SingleApi.Data.Contracts.Models
{
    public class TransactionPaymentReference
    {
        public string OperationId { get; set; }
        public string BankPaymentReference { get; set; }
        public string BankID { get; set; }
        public DateTime CreateDateTime { get; set; }
        public Guid OneStepTokenID { get; set; }
    }
}
