namespace SingleApi.Data.Contracts.Models
{
    public class OperationStatus
    {
        public const string Expired = "100";
        public const string Pending = "101";
        public const string AlreadyPaid = "102";
        public const string Concilied = "103";
        public const string Void = "120";
        public const string InProcess = "121";
    }
}
