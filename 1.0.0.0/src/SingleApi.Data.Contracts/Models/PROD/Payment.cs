namespace SingleApi.Data.Contracts.Models.PROD
{
    public class Payment
    {
        public string OperationID { get; set; }
        public string BankID { get; set; }
        public string BankOperationID { get; set; }
        public bool IsForced { get; set; }
        public DateTime CreationDateTime { get; set; }
    }
}
