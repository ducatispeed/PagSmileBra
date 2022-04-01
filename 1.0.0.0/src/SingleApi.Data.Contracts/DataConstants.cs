namespace SingleApi.Data.Contracts
{
    public sealed class DataConstants
    {
        public class StoredProcedures
        {
            public class Partner
            {
                public const string TransactionExistsByTokenAndPaymentReference = "[Integration].[uspTransactionExistsByTokenAndPaymentReference]";
                public const string GetTransactionByOperationId = "[Integration].[uspTransactionGetByOperationId]";
                public const string UspTransactionGetByTransactionPaymentReference = "[Integration].[uspTransactionGetByTransactionPaymentReference]";
                public const string UspTransactionStatusResponseLogInsert = "[Integration].[uspTransactionStatusResponseLogInsert]";
                public const string UspGetBankStatusIdByErrorCode = "[Integration].[uspGetBankStatusIdByErrorCode]";
                public const string UspTransactionUpdateByOperationId = "[Integration].[uspTransactionUpdateByOperationId]";
                public const string GetGatewayPspConfigurationsByBankAndChannel = "[Integration].[uspGetGatewayPspConfigurationsByBankAndChannel]";
                public const string UspGetGatewayPspConfigurationsByBank = "[Integration].[uspGetGatewayPspConfigurationsByBank]";
                public const string UspTransactionPaymentReferenceGetByPaymentReferenceAndBankId = "[Integration].[uspTransactionPaymentReferenceGetByPaymentReferenceAndBankId]";
            }

            public class Prod
            {
                public const string GetPaymentByOperationID = "[dbo].[GetPaymentByOperationID]";
                public const string GetOneStepTokensByTokenID = "[dbo].[GetOneStepTokensByTokenID]";
                public const string GetOperationByOperationId = "[dbo].[GetOperationByOperationId]";
                public const string GetMerchantInformationByOperationId = "[dbo].[uspGetMerchantInformationByOperationId]";
                public const string GetMerchantCssByOperationId = "[dbo].[uspGetMerchantCssByOperationId]";
                public const string GetCommissionFeeByOperationIdAndCurrencyId = "[dbo].[uspGetCommissionFeeByOperationIdAndCurrencyId]";
            }

            public class Payment
            {
                public const string GetPaymentInstructionsByPaymentChannelCode = "[dbo].[uspGetPaymentInstructionsByPaymentChannelCode]";
                public const string GetCountryConfiguration = "[dbo].[uspGetCountryConfigurations]";
            }

            public class BatchProcessDbW
            {
                public const string UspSyAppSvcConfigSettingsGet = "[Config].[uspSyAppSvcConfigSettingsGet]";
            }
        }
    }
}