using SingleApi.Data.Contracts.Base;
using SingleApi.Data.Contracts.Models.PROD;

namespace SingleApi.Data.Contracts.Shared
{
    public interface IProdRepository : IBaseRepository
    {
        Task<OneStepToken> GetOneStepTokenByOperationId(string operationId);
        Task<OneStepToken> GetOneStepTokenById(string oneStepTokenId);
        Task<OperationInfo> GetPaidOperationInfo(string operationId);
        Task<OperationInfo> GetOperationInfo(string operationId);
        Task<Operation> GetOperationById(string operationId, string country);
        Task<MerchantInfo> GetMerchantInfo(long merchantId, string acceptableImageFormats);
        Task<DateTime> GetCurrentLocaleDate(string countryCode);
        Task<Payment> GetPayment(string operationId);
        Task<Guid?> GetExpressToken(string operationId);
        Task<DateTime> GetExpirationDateTimeByOperation(string operationId);
        Task<string> GetMerchantName(string operationId);
        Task<string> GetShopperEmailFromExpressToken(string operationId);
        Task<string> GetShopperEmailFromCustomerInfo(string operationId);

        Task<Payment> OperationIsForced(string operationId);
        Task<string> GetMerchantCssByOperationId(string operationId);
        Task<string> GetBankNameByBankId(string bankId);
    }
}
