using SingleApi.Data.Contracts.Models;
using SingleApi.Svc.Contracts.Paysafe.Models.Requests;
using SingleApi.Svc.Contracts.Paysafe.Models.Responses;

namespace SingleApi.Svc.Contracts.Paysafe.Services
{
    public interface ICoreOperationsService
    {
        Task<PayInternalTransactionResponse> PayTransaction(PayInternalTransactionRequest request);
    }
}
