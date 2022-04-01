using SingleApi.Infrastructure.Enums;

namespace SingleApi.Data.Contracts.Models.PROD
{
    public class OneStepToken
    {
        public Guid TokenId { get; set; }
        public string BankId { get; set; }
        public string OperationId { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string LastStatus { get; set; }
        public string TransactionOkUrl { get; set; }
        public string TransactionErrorUrl { get; set; }
        public Channel ChannelId { get; set; }
        public string Language { get; set; }
    }
}
