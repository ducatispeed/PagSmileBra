using Dapper;
using SingleApi.Data.Base;
using SingleApi.Data.Contracts.Models.PROD;
using SingleApi.Data.Contracts.Shared;
using System.Data;

namespace SingleApi.Data.Shared
{
    public class ProdRepository : BaseRepository, IProdRepository
    {
        public ProdRepository(string connectionString) : base(connectionString)
        {
        }

        public async Task<OneStepToken> GetOneStepTokenByOperationId(string operationId)
        {
            string sql = @"SELECT TOP 1 [TokenID], [BankID], [OperationID], [CreationDate], [ExpirationDate], [LastStatus], [TransactionOkURL], 
                            [TransactionErrorURL], [ChannelID], [Language] FROM [dbo].[OneStepTokens] WHERE [OperationID] = @operationId ORDER BY [CreationDate] DESC";

            var parameters = new DynamicParameters();
            parameters.Add("operationId", operationId, System.Data.DbType.AnsiString);

            return await GetFirstOrDefaultAsync<OneStepToken>(sql, parameters);
        }

        public async Task<OneStepToken> GetOneStepTokenById(string oneStepTokenId)
        {
            string sql = @"SELECT [TokenID], [BankID], [OperationID], [CreationDate], [ExpirationDate], [LastStatus], [TransactionOkURL], 
                            [TransactionErrorURL], [ChannelID], [Language] FROM [dbo].[OneStepTokens] WHERE [TokenID] = @tokenId";

            var parameters = new DynamicParameters();
            parameters.Add("tokenId", oneStepTokenId);

            return await GetFirstOrDefaultAsync<OneStepToken>(sql, parameters);
        }

        public async Task<OperationInfo> GetPaidOperationInfo(string operationId)
        {
            string sql = @"SELECT TOP 1  T.TransactionIdentifier AS 'TransactionId',
M.MERCHANT_NAME AS 'MerchantName',
P.BankOperationID,
C.Symbol AS 'CurSymbol',
O.ToAmount AS 'Amount'
FROM [dbo].[Operation] O
JOIN [dbo].[Merchants] M ON M.Id = O.MerchantId
JOIN [dbo].[Transactions] T ON T.ReferenceNo = O.OperationId
JOIN [dbo].[Currency] C (NOLOCK) ON C.CURRENCY_CODE COLLATE DATABASE_DEFAULT = O.ToCurSymbol
LEFT JOIN [dbo].[Payment] P (NOLOCK) ON P.OperationID = O.OperationId
WHERE O.[OperationID] = @operationId";

            var parameters = new DynamicParameters();
            parameters.Add("operationId", operationId, System.Data.DbType.AnsiString);

            return await GetFirstOrDefaultAsync<OperationInfo>(sql, parameters);
        }

        public async Task<OperationInfo> GetOperationInfo(string operationId)
        {
            string sql = @"SELECT TOP 1  T.TransactionIdentifier AS 'TransactionId',
M.MERCHANT_NAME AS 'MerchantName',
M.MERCHANT_ID AS 'MerchantCode',
O.ToCurSymbol AS 'CurSymbol',
O.ToAmount AS 'Amount',
T.TRANS_EXPIRATION AS 'ExpirationDateTime'
FROM [dbo].[Operation] O
JOIN [dbo].[Merchants] M ON M.Id = O.MerchantId
JOIN [dbo].[Transactions] T ON T.ReferenceNo = O.OperationId
WHERE O.[OperationID] = @operationId";

            var parameters = new DynamicParameters();
            parameters.Add("operationId", operationId, System.Data.DbType.AnsiString);

            return await GetFirstOrDefaultAsync<OperationInfo>(sql, parameters);
        }

        public async Task<Operation> GetOperationById(string operationId, string country)
        {
            string sql = @"SELECT TOP 1 [dbo].[udf_GetCountryTime](@country, T.[TRANS_EXPIRATION]) AS 'ExpirationTime', O.[RecordDate], O.[OperationId], O.[MerchantId], O.[TransId], O.[CurSymbol], O.[Amount], O.[ToAmount], O.[ToCurSymbol], O.[LastStatus], O.[Completed],
                            O.[LastChange], O.[ForexRate], O.[MarkUp], O.[MerchantDiscountRate], O.[ProcessingFee], O.[MerchantFeeFormula], O.[NetToMerchant], O.[NetToBank], O.[ShopperId],
                            [dbo].[udf_GetCountryTime](@country, T.[SHOPPER_EXPIRATION]) AS 'ShopperExpirationTime'
                            FROM [dbo].[Operation] O
                            JOIN [dbo].[Transactions] T ON T.ReferenceNo = O.OperationID
                            WHERE [OperationId] = @operationId";

            var parameters = new DynamicParameters();
            parameters.Add("operationId", operationId, System.Data.DbType.AnsiString);
            parameters.Add("country", country, System.Data.DbType.AnsiString);

            return await GetFirstOrDefaultAsync<Operation>(sql, parameters);
        }

        public async Task<MerchantInfo> GetMerchantInfo(long merchantId, string acceptableImageFormats)
        {
            IEnumerable<string> imageExtensions = acceptableImageFormats.Split(',').Select(p => p.Trim()).ToList();

            string sql = @"SELECT m.MERCHANT_ID AS [Code], m.MERCHANT_NAME AS [Name], ImagePhysicalName AS [Logo] , m.MERCHANT_COUNTRY AS Country, m.MERCHANT_URL AS Url       
            FROM dbo.Merchants m
							LEFT JOIN dbo.MerchantBranding mb ON m.Id=mb.MerchantId AND ImageExtension IN @AcceptableImageFormats
							WHERE Id = @MerchantId;";

            var parameters = new DynamicParameters();
            parameters.Add("MerchantId", merchantId, System.Data.DbType.Int64);
            parameters.Add("AcceptableImageFormats", imageExtensions);

            return await GetFirstOrDefaultAsync<MerchantInfo>(sql, parameters);
        }

        public async Task<DateTime> GetCurrentLocaleDate(string countryCode)
        {
            string sql = @"SELECT [dbo].[udf_GetCountryTime](@countryCode, GETDATE())";

            var parameters = new DynamicParameters();
            parameters.Add("countryCode", countryCode, System.Data.DbType.AnsiString);

            return await ExecuteScalarAsync<DateTime>(sql, parameters);
        }

        public async Task<Payment> GetPayment(string operationId)
        {
            var param = new DynamicParameters();
            param.Add("@OperationId", operationId, System.Data.DbType.AnsiStringFixedLength, size: 16);

            return await GetFirstOrDefaultAsync<Payment>("[dbo].[GetPaymentByOperationID]", param, CommandType.StoredProcedure);
        }

        public async Task<Guid?> GetExpressToken(string operationId)
        {
            var sql = @"SELECT TOP 1 [TokenId] FROM [dbo].[ExpressTokens] WHERE [OperationID] = @OperationId";
            var param = new DynamicParameters();
            param.Add("@OperationId", operationId, System.Data.DbType.AnsiStringFixedLength, size: 16);
            return await QuerySingleAsync<Guid?>(sql, param);
        }

        public async Task<DateTime> GetExpirationDateTimeByOperation(string operationId)
        {
            string sql = @"SELECT [ExpirationDatetime] FROM [dbo].[ActiveTransactions] WHERE [OperationID] = @operationId";

            var parameters = new DynamicParameters();
            parameters.Add("operationId", operationId);

            return await GetFirstOrDefaultAsync<DateTime>(sql, parameters);
        }

        public async Task<string> GetMerchantName(string operationId)
        {
            string sql = @"[dbo].[getMerchantNameByOperationId]";

            var parameters = new DynamicParameters();
            parameters.Add("@OperationId", operationId, DbType.AnsiStringFixedLength, size: 16);

            return await ExecuteScalarAsync<string>(sql, parameters, CommandType.StoredProcedure);
        }

        public async Task<Payment> OperationIsForced(string operationId)
        {
            var sql = @"SELECT [OperationID],[BankID],[BankOperationID],[ChannelID],[IsForced],[IsAutomaticPushPay] FROM [dbo].[Payment] WITH(NOLOCK) WHERE OperationID = @OperationId";
            var param = new DynamicParameters();
            param.Add("@OperationId", operationId, System.Data.DbType.AnsiStringFixedLength, size: 16);
            return await GetFirstOrDefaultAsync<Payment>(sql, param);
        }

        public async Task<string> GetShopperEmailFromExpressToken(string operationId)
        {
            var sql = @"SELECT [ShopperEmail] FROM [dbo].[ExpressTokens] WITH(NOLOCK) WHERE OperationID = @OperationId";
            var param = new DynamicParameters();
            param.Add("@OperationId", operationId, System.Data.DbType.AnsiStringFixedLength, size: 16);
            return await GetFirstOrDefaultAsync<string>(sql, param);
        }

        public async Task<string> GetShopperEmailFromCustomerInfo(string operationId)
        {
            string sql = @"[dbo].[GetDirectCustomerInformationByOperationId]";

            var parameters = new DynamicParameters();
            parameters.Add("@OperationId", operationId, DbType.AnsiStringFixedLength, size: 16);

            var result = await GetFilteredListAsync<CustomerInfo>(sql, parameters, CommandType.StoredProcedure);
            return result.FirstOrDefault(x => x.FieldName == "CustomerEmail")?.FieldValue;
        }

        public async Task<string> GetMerchantCssByOperationId(string operationId)
        {
            string sql = @"[dbo].[uspGetMerchantCssByOperationId]";

            var parameters = new DynamicParameters();
            parameters.Add("operationId", operationId, DbType.AnsiStringFixedLength, ParameterDirection.Input, 16);

            return await GetFirstOrDefaultAsync<string>(sql, parameters, CommandType.StoredProcedure).ConfigureAwait(false);
        }

        public async Task<string> GetBankNameByBankId(string bankId)
        {
            var sql = @"SELECT [BANK_NAME] FROM [dbo].[Banks] WITH(NOLOCK) WHERE [BANK_ID] = @BankId";
            var param = new DynamicParameters();
            param.Add("@BankId", bankId, DbType.AnsiStringFixedLength, ParameterDirection.Input, 4);
            return await GetFirstOrDefaultAsync<string>(sql, param);
        }

        private class CustomerInfo
        {
            public string FieldName { get; set; }
            public string FieldValue { get; set; }
        }
    }
}
