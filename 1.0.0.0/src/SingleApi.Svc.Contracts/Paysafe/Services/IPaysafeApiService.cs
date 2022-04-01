using SingleApi.Data.Contracts.Models;
using SingleApi.Svc.Contracts.Paysafe.Models.Requests;
using SingleApi.Svc.Contracts.Paysafe.Models.Responses;

namespace SingleApi.Svc.Contracts.Paysafe.Services
{
    public interface IPaysafeApiService
    {
        Task<PostPaymentHandleResponse> PostPaymentHandleAsync(PostPaymentHandleRequest request, IEnumerable<GatewayPspConfiguration> config);
        Task<GetPaymentStatusHandleResponse?> GetPaymentStatusHandleAsync(GetPaymentStatusHandleRequest request, IEnumerable<GatewayPspConfiguration> config);
        Task<PostPaymentHubHandleResponse?> PostPaymentHubHandleAsync(PostPaymentHubHandleRequest request, IEnumerable<GatewayPspConfiguration> config);
    }
}
