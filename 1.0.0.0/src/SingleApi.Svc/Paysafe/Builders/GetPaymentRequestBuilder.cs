
using SingleApi.Data.Contracts.Models.PROD;
using SingleApi.Infrastructure.Constants;
using SingleApi.Svc.Contracts.Paysafe.Models.Requests;
using static SingleApi.Svc.Contracts.Paysafe.Models.Requests.GetPaymentStatusHandleRequest;

namespace GlobalEditAPI.Assets.Core.Tests.Builders
{
    public class GetPaymentRequestBuilder
    {
        private GetPaymentStatusHandleRequest request= new GetPaymentStatusHandleRequest();

        public GetPaymentStatusHandleRequest Build()
        {
            return request;
        }
        public GetPaymentRequestBuilder WithPaymentId(string paymentId) 
        {
            request.PaymentId = paymentId;
            return this;
        }
        public GetPaymentRequestBuilder WithTestValues(OperationInfo operationInfo,OneStepToken oneStepToken)
        {
            request.Amount = operationInfo.Amount;
            request.CurrencyCode = operationInfo.CurSymbol;
            request.PaymentType = PaymentRequest.PaymentType;
            request.MerchantRefNum = Guid.NewGuid().ToString();
            request.TransactionType = PaymentRequest.TransactionType;
            request.ReturnLinks = new List<ReturnLinkModel>
                {
                    new ReturnLinkModel
                    {
                        Rel = PaymentRequest.OnCompletedCallback,
                        Method = PaymentRequest.CallbackMethod,
                        Href = oneStepToken.TransactionOkUrl,//"https://us_commerce_site/payment/return/success"
                    },
                    new ReturnLinkModel
                    {
                        Rel = PaymentRequest.OnFailedCallback,
                        Method = PaymentRequest.CallbackMethod,
                        Href = oneStepToken.TransactionErrorUrl,//"https://us_commerce_site/payment/return/failed"
                    }
                };

            request.CustomerIp = "73.82.192.17";
            request.RapidTransfer = new RapidTransferModel
                                {
                                    ConsumerId = "john@gmail.com",
                                    CountryCode = "IE"
                                };
            request.Profile = new ProfileModel
                                {
                                    FirstName = "Ivan",
                                    LastName = "Ivanov"
                                };
            request.BillingDetails = new BillingDetailsModel
                                {
                                    Street = "Nemiga",
                                    City = "Minsk",
                                    Country = "GB",
                                    Zip = "SO53 5PD"
                                };
            return this;
        }
    }
}
