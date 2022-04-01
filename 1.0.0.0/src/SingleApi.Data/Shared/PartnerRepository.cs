using Dapper;
using SingleApi.Data.Base;
using SingleApi.Data.Contracts.Models;
using SingleApi.Data.Contracts.Models.Partner;
using SingleApi.Data.Contracts.Shared;
using System.Data;
using SingleApi.Infrastructure.Enums;

namespace SingleApi.Data.Shared
{
    public class PartnerRepository : BaseRepository, IPartnerRepository
    {
        public PartnerRepository(string connectionString) : base(connectionString)
        {
        }

        public async Task<IEnumerable<GatewayPspConfiguration>> GetConfigurations(string gatewayPspIdentifier)
        {
            string sql = @"SELECT [GPC].[Id], [GPC].[GatewayPspId], [GPC].[BankId], [GPC].[ConfigurationName], [GPC].[Value], [GPC].[ChannelTypeId], [GPC].[DateCreated], [GPC].[DateModified] 
                            FROM [Integration].[GatewayPspConfiguration] [GPC] RIGHT JOIN [Integration].[GatewayPsp] [GP]
                            ON [GP].[Id] = [GPC].[GatewayPspId]
                            WHERE [GP].[Identifier] = @gatewayPspIdentifier AND 
                            ([GPC].[BankId] = @bankId OR [GPC].[BankId] IS NULL) ORDER BY [GPC].[BankId] DESC";

            var parameters = new DynamicParameters();
            parameters.Add("bankId", null, DbType.AnsiStringFixedLength, ParameterDirection.Input, 4);
            parameters.Add("gatewayPspIdentifier", gatewayPspIdentifier, DbType.AnsiString, ParameterDirection.Input, 20);

            return await GetFilteredListAsync<GatewayPspConfiguration>(sql, parameters);
        }

        public async Task<Transaction> GetTransactionByOperationId(string operationId)
        {
            string sql = @"SELECT [OperationID], [TransactionID], [BankPaymentReference], [ChannelID], [OneStepTokenID], [ProdStatus], 
                            [BankOperationID], [IsProcessed], [Amount], [CurrencyID], [BankID], [ExpirationDateTime], [IsForced], [BarCodeURL], 
                            [PaymentDateTime], [AuthorizationCode], [BankStatusID], [CreateDateTime], [BanksPSPPaymentUrl]
                            FROM [Integration].[Transaction] WHERE [OperationID] = @operationId";

            var parameters = new DynamicParameters();
            parameters.Add("operationId", operationId, DbType.AnsiStringFixedLength, ParameterDirection.Input, 16);

            return await GetFirstOrDefaultAsync<Transaction>(sql, parameters);
        }

        public async Task<Transaction> GetTransactionByPaymentReference(string paymentReference, string bankId)
        {
            string sp = "[Integration].[uspTransactionGetByTransactionPaymentReference]";
            var parameters = new DynamicParameters();
            parameters.Add("paymentReference", paymentReference, DbType.String, ParameterDirection.Input, 50);
            parameters.Add("bankId", bankId, DbType.AnsiStringFixedLength, ParameterDirection.Input, 4);

            return await GetFirstOrDefaultAsync<Transaction>(sp, parameters, commandType: CommandType.StoredProcedure);
        }

        public async Task UpdateTransactionAsync(Transaction transaction)
        {
            var parameters = new DynamicParameters();

            parameters.Add("OperationID", transaction.OperationId, DbType.AnsiStringFixedLength, ParameterDirection.Input, 16);
            parameters.Add("BankPaymentReference", transaction.BankPaymentReference, DbType.AnsiString, ParameterDirection.Input, 50);
            parameters.Add("ChannelID", (short)transaction.ChannelId, DbType.Int16, ParameterDirection.Input);
            parameters.Add("OneStepTokenID", transaction.OneStepTokenId, DbType.Guid, ParameterDirection.Input);
            parameters.Add("ProdStatus", transaction.ProdStatus, DbType.AnsiString, ParameterDirection.Input, 10);
            parameters.Add("BankOperationID", transaction.BankOperationId, DbType.AnsiString, ParameterDirection.Input, 50);
            parameters.Add("IsProcessed", transaction.IsProcessed, DbType.Boolean, ParameterDirection.Input);
            parameters.Add("BankID", transaction.BankId, DbType.AnsiStringFixedLength, ParameterDirection.Input, 4);
            parameters.Add("ExpirationDateTime", transaction.ExpirationDateTime, DbType.DateTime, ParameterDirection.Input);
            parameters.Add("IsForced", transaction.IsForced, DbType.Boolean, ParameterDirection.Input);
            parameters.Add("BarCodeURL", transaction.BarCodeUrl, DbType.String, ParameterDirection.Input, 255);
            parameters.Add("PaymentDateTime", transaction.PaymentDateTime, DbType.DateTime, ParameterDirection.Input);
            parameters.Add("BankStatusID", transaction.BankStatusId, DbType.Int16, ParameterDirection.Input);
            parameters.Add("ActualPaymentBankID", transaction.ActualPaymentBankID, DbType.AnsiStringFixedLength, ParameterDirection.Input, 4);

            await ExecuteAsync("[Integration].[uspTransactionUpdateByOperationId]", parameters, CommandType.StoredProcedure);
        }

        public async Task AddTransactionStatusResponseLog(Transaction transaction, string description, BankStatus bankStatus)
        {
            var sql = @"INSERT INTO [Integration].[TransactionStatusResponseLog] ([TokenID], [OperationID], [Description], [ErrorCode], [BankID], [CreateDateTime])
VALUES (@tokenId, @operationId, @description, @errorCode, @bankId, GETDATE())";

            var parameters = new DynamicParameters();
            parameters.Add("tokenId", transaction.OneStepTokenId, DbType.Guid);
            parameters.Add("operationId", transaction.OperationId, DbType.AnsiString, size: 16);
            parameters.Add("description", description, DbType.AnsiString);
            parameters.Add("errorCode", ((int)bankStatus).ToString(), DbType.AnsiString);
            parameters.Add("bankId", transaction.BankId, DbType.AnsiString, size: 4);

            await ExecuteAsync(sql, parameters);
        }

        public async Task<short> GetBankStatusId(string bankId, int bankStatus)
        {
            string sql = @"SELECT [BankStatusID] FROM [Integration].[BankStatus] WHERE [BankID] = @bankId AND [ErrorCode] = @errorCode;";

            var parameters = new DynamicParameters();
            parameters.Add("bankId", bankId, DbType.AnsiStringFixedLength, size: 4);
            parameters.Add("errorCode", bankStatus, DbType.AnsiString);

            return await GetShortAsync(sql, parameters);
        }

        public async Task CreateTransaction(Transaction transaction)
        {
            string sql = @"INSERT INTO [Integration].[Transaction] ([OperationID], [TransactionID], [BankPaymentReference], [ChannelID],
                            [OneStepTokenID], [ProdStatus], [BankOperationID], [IsProcessed], [Amount], [CurrencyID], [BankID], [ExpirationDateTime],
                            [IsForced], [BarCodeURL], [PaymentDateTime], [AuthorizationCode], [BankStatusID], [CreateDateTime], [BanksPSPPaymentUrl])
                            VALUES (@operationId, @transactionId, @bankPaymentReference, @channelId,
                            @oneStepTokenId, @prodStatus, @bankOperationId, @isProcessed, @amount, @currencyId, @bankId, @expirationDateTime,
                            @isForced, @barCodeUrl, @paymentDateTime, @authorizationCode, @bankStatusId, GETDATE(), @banksPSPPaymentUrl);";

            var parameters = new DynamicParameters();

            parameters.Add("operationId", transaction.OperationId, DbType.AnsiString, size: 16);
            parameters.Add("transactionId", transaction.TransactionId, DbType.AnsiString, size: 50);
            parameters.Add("bankPaymentReference", transaction.BankPaymentReference, DbType.String, size: 50);
            parameters.Add("channelId", transaction.ChannelId, DbType.Int16);
            parameters.Add("oneStepTokenId", transaction.OneStepTokenId, DbType.Guid);
            parameters.Add("prodStatus", transaction.ProdStatus, DbType.AnsiString, size: 10);
            parameters.Add("bankOperationId", transaction.BankOperationId, DbType.AnsiString, size: 50);
            parameters.Add("isProcessed", transaction.IsProcessed, DbType.Boolean);
            parameters.Add("amount", transaction.Amount, DbType.Decimal);
            parameters.Add("currencyId", transaction.CurrencyId, DbType.AnsiStringFixedLength, size: 3);
            parameters.Add("bankId", transaction.BankId, DbType.AnsiStringFixedLength, size: 4);
            parameters.Add("expirationDateTime", transaction.ExpirationDateTime, DbType.DateTime);
            parameters.Add("isForced", transaction.IsForced, DbType.Boolean);
            parameters.Add("barCodeUrl", transaction.BarCodeUrl, DbType.String, size: 255);
            parameters.Add("paymentDateTime", transaction.PaymentDateTime, DbType.DateTime);
            parameters.Add("authorizationCode", transaction.AuthorizationCode, DbType.String, size: 50);
            parameters.Add("bankStatusId", transaction.BankStatusId, DbType.Int16);
            parameters.Add("banksPSPPaymentUrl", transaction.BanksPspPaymentUrl, DbType.String, size: 1024);

            await ExecuteAsync(sql, parameters);
        }

        public async Task UpdateTransaction(Transaction transaction)
        {
            string sql = @"UPDATE [Integration].[Transaction] SET [TransactionID] = @transactionId, [BankPaymentReference] = @bankPaymentReference, [ChannelID] = @channelId, 
                            [OneStepTokenID] = @oneStepTokenId, [ProdStatus] = @prodStatus, [BankOperationID] = @bankOperationId, [IsProcessed] = @isProcessed,
                            [Amount] = @amount, [CurrencyID] = @currencyId, [BankID] = @bankId, [ExpirationDateTime] = @expirationDateTime, [IsForced] = @isForced, 
                            [BarCodeURL] = @barCodeUrl, [PaymentDateTime] = @paymentDateTime, [AuthorizationCode] = @authorizationCode, [BankStatusID] = @bankStatusId, 
                            [BanksPSPPaymentUrl] = @banksPSPPaymentUrl, [ActualPaymentBankId]='', [BankPaymentSlipIssuingBankId]='', [BankPaymentSlipSerialNumber]='', [BankPaymentSlipIssuingDate]=NULL
                            WHERE [OperationID] = @operationId";

            var parameters = new DynamicParameters();

            parameters.Add("operationId", transaction.OperationId, DbType.AnsiString, size: 16);
            parameters.Add("transactionId", transaction.TransactionId, DbType.AnsiString, size: 50);
            parameters.Add("bankPaymentReference", transaction.BankPaymentReference, DbType.String, size: 50);
            parameters.Add("channelId", transaction.ChannelId, DbType.Int16);
            parameters.Add("oneStepTokenId", transaction.OneStepTokenId, DbType.Guid);
            parameters.Add("prodStatus", transaction.ProdStatus, DbType.AnsiString, size: 10);
            parameters.Add("bankOperationId", transaction.BankOperationId, DbType.AnsiString, size: 50);
            parameters.Add("isProcessed", transaction.IsProcessed, DbType.Boolean);
            parameters.Add("amount", transaction.Amount, DbType.Decimal);
            parameters.Add("currencyId", transaction.CurrencyId, DbType.AnsiStringFixedLength, size: 3);
            parameters.Add("bankId", transaction.BankId, DbType.AnsiStringFixedLength, size: 4);
            parameters.Add("expirationDateTime", transaction.ExpirationDateTime, DbType.DateTime);
            parameters.Add("isForced", transaction.IsForced, DbType.Boolean);
            parameters.Add("barCodeUrl", transaction.BarCodeUrl, DbType.String, size: 255);
            parameters.Add("paymentDateTime", transaction.PaymentDateTime, DbType.DateTime);
            parameters.Add("authorizationCode", transaction.AuthorizationCode, DbType.String, size: 50);
            parameters.Add("bankStatusId", transaction.BankStatusId, DbType.Int16);
            parameters.Add("banksPSPPaymentUrl", transaction.BanksPspPaymentUrl, DbType.String, size: 1024);

            await ExecuteAsync(sql, parameters);
        }

        public async Task AddTransactionStatusResponseLog(string oneStepTokenId, string operationId, string bankId, string description, string errorCode, string componentName)
        {
            string sql = @"INSERT INTO [Integration].[TransactionStatusResponseLog] ([TokenID], [OperationID], [Description], [ErrorCode], [BankID], [CreateDateTime])
                            VALUES (@tokenId, @operationId, @description, @errorCode, @bankId, GETDATE())";

            var parameters = new DynamicParameters();
            parameters.Add("tokenId", oneStepTokenId, DbType.Guid);
            parameters.Add("operationId", operationId, DbType.AnsiString, size: 16);
            parameters.Add("description", description, DbType.AnsiString);
            parameters.Add("errorCode", errorCode, DbType.AnsiString);
            parameters.Add("bankId", bankId, DbType.AnsiString, size: 4);


            await ExecuteAsync(sql, parameters);
        }

        public async Task<TransactionPaymentReference> GetTransactionPaymentReference(string paymentReference, string bankCode)
        {
            var parameters = new DynamicParameters();
            parameters.Add("paymentReference", paymentReference, DbType.String, ParameterDirection.Input, 50);
            parameters.Add("bankId", bankCode, DbType.AnsiStringFixedLength, ParameterDirection.Input, 4);

            return await GetFirstOrDefaultAsync<TransactionPaymentReference>("[Integration].[uspTransactionPaymentReferenceGetByPaymentReferenceAndBankId]", parameters, CommandType.StoredProcedure);
        }

        public async Task<TransactionPaymentReference> SaveTransactionPaymentReference(string operationID, string bankPaymentReference, string bankID, Guid oneStepTokenId)
        {
            var param = new DynamicParameters();
            param.Add("@OperationID", operationID);
            param.Add("@BankPaymentReference", bankPaymentReference);
            param.Add("@BankID", bankID);
            param.Add("@OneStepTokenID", oneStepTokenId);

            return await GetFirstOrDefaultAsync<TransactionPaymentReference>("[Integration].[uspTransactionPaymentReferenceInsert]", param, CommandType.StoredProcedure);
        }

        public async Task<BankMapping> GetBankMapping(string operationId)
        {
            string sql = @"SELECT [CountryId],[BankId],[IntegrationBankCode]
                            FROM [Integration].[BankMapping]
                            WHERE [IntegrationBankCode] = @integrationBankCode";

            var parameters = new DynamicParameters();
            parameters.Add("integrationBankCode", operationId, DbType.AnsiStringFixedLength, ParameterDirection.Input, 50);

            return await GetFirstOrDefaultAsync<BankMapping>(sql, parameters);
        }

        public async Task<IEnumerable<BankMapping>> GetBankMappingIdsByBankId(string bankId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("GatewayPspCode", bankId);

            return await GetFilteredListAsync<BankMapping>("[Integration].[uspBankMappingGetByGatewayPspCode]", parameters, CommandType.StoredProcedure);
        }
    }
}
