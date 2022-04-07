using SingleApi.Infrastructure.Enums;

namespace SingleApi.Infrastructure.Mappers
{
    public static class ErrorMapper
    {
        public static string MapResponseCode(ErrorNumber errorNumber)
        {
            return MapResponseCode((int)errorNumber);
        }

        public static string MapResponseCode(int errorNumber)
        {
            switch (errorNumber)
            {
                case (int)ErrorNumber.UnexpectedError: return "UnexpectedError";

                case (int)ErrorNumber.InvalidPaymentDate: return "The payment date is greater than the current DateTime of Processing";
                case (int)ErrorNumber.SuccessTransaction: return "Successful Transaction";
                case (int)ErrorNumber.TransactionExpired: return "Transaction Expired";
                case (int)ErrorNumber.TransactionAlreadyPaid: return "Transaction already Paid";
                case (int)ErrorNumber.VoidTransaction: return "Void Transaction";
                case (int)ErrorNumber.TransactionIsNotExists: return "Transaction is not exists. out_trade_no: {0}";
                case (int)ErrorNumber.TemplateNotFound: return "Template not found {0}";

            }
            throw new ArgumentException($"ErrorMapper fails to find description to code {errorNumber}");
        }
    }
}
