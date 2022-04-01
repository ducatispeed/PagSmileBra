namespace SingleApi.Data.Contracts.Models.PROD
{
    public class Operation
    {
        public DateTime RecordDate { get; set; }
        public string OperationId { get; set; }
        public long MerchantId { get; set; }
        public string TransId { get; set; }
        public string CurSymbol { get; set; }
        public decimal Amount { get; set; }
        public decimal ToAmount { get; set; }
        public string ToCurSymbol { get; set; }
        public string LastStatus { get; set; }
        public bool Completed { get; set; }
        public DateTime LastChange { get; set; }
        public decimal ForexRate { get; set; }
        public decimal MarkUp { get; set; }
        public decimal MerchantDiscountRate { get; set; }
        public decimal ProcessingFee { get; set; }
        public string MerchantFeeFormula { get; set; }
        public DateTime ExpirationTime { get; set; }
        public decimal? NetToMerchant { get; set; }
        public decimal? NetToBank { get; set; }
        public int? ShopperId { get; set; }
        public DateTime LastStatusCreationDate { get; set; }
        public DateTime ShopperExpirationTime { get; set; }
    }
}
