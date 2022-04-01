using SingleApi.Infrastructure.Enums;

namespace SingleApi.Data.Contracts.Models
{
    public class Transaction
    {
        public string OperationId { get; set; }
        public string TransactionId { get; set; }
        public string BankPaymentReference { get; set; }
        public Channel ChannelId { get; set; }
        public Guid OneStepTokenId { get; set; }
        public string ProdStatus { get; set; }
        public string BankOperationId { get; set; }
        public bool IsProcessed { get; set; }
        public decimal Amount { get; set; }
        public string CurrencyId { get; set; }
        public string BankId { get; set; }
        public DateTime? ExpirationDateTime { get; set; }
        public bool IsForced { get; set; }
        public string BarCodeUrl { get; set; }
        public DateTime? PaymentDateTime { get; set; }
        public string AuthorizationCode { get; set; }
        public short? BankStatusId { get; set; }
        public DateTime CreateDateTime { get; set; }
        public string BanksPspPaymentUrl { get; set; }
        public string ActualPaymentBankID { get; set; }
    }
}
