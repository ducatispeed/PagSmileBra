namespace SingleApi.Svc.Contracts.Paysafe.Models.Responses
{
    public class PayInternalTransactionResponse
    {
        public bool Succeeded { get; set; }
        public int ErrorNumber { get; set; }
        public string ErrorDescription { get; set; }
    }
}
