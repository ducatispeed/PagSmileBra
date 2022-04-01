using MediatR;
using SfpCoreLib.Domain.Commands;
using SfpCoreLib.Domain.Validation;
using SingleApi.Svc.Contracts.Paysafe.Models.View;

namespace SingleApi.Svc.Contracts.Paysafe.Commands
{
    public class GetPaymentStatusHandleCommand : CommandBase, IRequest<Result<GetPaymentStatusHandleViewModel>>
    {
        public string PaymentId { get; set; }
        public string OperationId { get; set; }
    }
}
