using SingleApi.Data.Contracts.Base;
using SingleApi.Data.Contracts.Models;
using SingleApi.Data.Contracts.Models.Partner;
using SingleApi.Infrastructure.Enums;

namespace SingleApi.Data.Contracts.Shared
{
    public interface IPartnerRepository : IBaseRepository
    {
        Task<IEnumerable<GatewayPspConfiguration>> GetConfigurations(string gatewayPspIdentifier);
        Task<Transaction> GetTransactionByOperationId(string operationId);

        Task<Transaction> GetTransactionByPaymentReference(string paymentReference, string bankId);
        Task UpdateTransactionAsync(Transaction transaction);
        Task AddTransactionStatusResponseLog(Transaction transaction, string description, BankStatus bankStatus);
        Task AddTransactionStatusResponseLog(string oneStepTokenId, string operationId, string bankId, string description, string errorCode, string componentName);
        Task<short> GetBankStatusId(string bankId, int bankStatus);

        Task CreateTransaction(Transaction transaction);
        Task UpdateTransaction(Transaction transaction);
        Task<TransactionPaymentReference> GetTransactionPaymentReference(string paymentReference, string bankCode);
        Task<TransactionPaymentReference> SaveTransactionPaymentReference(string operationID, string bankPaymentReference, string bankID, Guid oneStepTokenId);
        Task<BankMapping> GetBankMapping(string operationId);
        
        Task<IEnumerable<BankMapping>> GetBankMappingIdsByBankId(string bankId);
        Task<string> GetOperationIdByPayloadId(string payloadId);
    }
}
