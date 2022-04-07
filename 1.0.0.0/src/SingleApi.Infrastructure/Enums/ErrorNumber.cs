
namespace SingleApi.Infrastructure.Enums
{
    public enum ErrorNumber
    {
        None = 0,

        UnexpectedError = 500,

        EventUnknown = 301,
        OperationIdNotRelatedToBradescoPIX = 302,
        AmountNotCoincideWithOperationId = 303,
        TransactionStatusNotAllowed = 304,
        ErrorInSondaRequest = 305,
        PaymentStatusNotRecognize = 306,
        NotPossibleDecrypt = 307,
        Expected4Elements = 308,

        SuccessTransaction = 310,
        TransactionExpired = 311,
        TransactionAlreadyPaid = 312,
        VoidTransaction = 313,
        TransactionIsNotExists = 314,
        TransactionReversed = 315,
        TransactionRefunded = 316,

        TemplateNotFound = 330,
        InvalidPaymentDate = 331,

        TokenIdIsMissig = 1,
        MaxLimitError = 2,
        MinLimitError = 3,
        WorkingHoursError = 4,
        OneStepTokenIsInvalid = 5,
        OperationStatusIsInvalid = 6
    }
}
