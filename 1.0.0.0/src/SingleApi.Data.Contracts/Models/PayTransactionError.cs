namespace SingleApi.Data.Contracts.Models
{
    public class PayTransactionError
    {
        public const int Success = 0;
        public const int AlreadyPaid = 10211;
        public const int Void = 10212;
        public const int Expired = 10203;
        public const int Expired_2 = 10209;
    }
}
