using SingleApi.Infrastructure.Enums;

namespace SingleApi.Svc.Contracts.Paysafe.Models.Responses
{
    public class GetPaymentStatusHandleResponse
    {
        public string Status { get; set; }
       
        public BaseResponce Error { get; set; }

        public class BaseResponce
        {
            public bool IsSuccess { get; set; }
            public string ErrorMessage { get; set; }
            public ErrorTypes ErrorCode { get; set; }
        }
    }
}
