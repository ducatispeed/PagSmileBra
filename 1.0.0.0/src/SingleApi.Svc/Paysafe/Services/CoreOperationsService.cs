using CoreWS;
using SingleApi.Svc.Contracts.Paysafe.Models.Requests;
using SingleApi.Svc.Contracts.Paysafe.Models.Responses;
using SingleApi.Svc.Contracts.Paysafe.Services;

namespace SingleApi.Svc.Paysafe.Services
{
    public class CoreOperationsService : ICoreOperationsService
    {
        public CoreOperationsService()
        {
        }

        public async Task<PayInternalTransactionResponse> PayTransaction(PayInternalTransactionRequest request)
        {
            ServiceEntryContract serviceEntryContract = new ServiceEntryContractClient(
                ServiceEntryContractClient.EndpointConfiguration.ServiceEntryContract, "CoreWsUrl");

            var response = await serviceEntryContract.PayTransactionAsync(new PayTransactionRequest
            {
                BankID = request.BankId,
                BankReference = request.BankReference,
                Channel = (short)request.ChannelId,
                ChannelSpecified = true,
                SalesOperationID = request.OperationId,
                ShopperAmount = request.ShopperAmount,
                ShopperCurrencyID = request.ShopperCurrencyId,
                IsForced = request.IsForced,
                IsForcedSpecified = request.IsForced
            });

            return new PayInternalTransactionResponse()
            {
                Succeeded = response.ErrorManager.ErrorNumber.Equals("0", StringComparison.OrdinalIgnoreCase),
                ErrorNumber = Convert.ToInt32(response.ErrorManager.ErrorNumber),
                ErrorDescription = response.ErrorManager.Description
            };
        }
    }

}
