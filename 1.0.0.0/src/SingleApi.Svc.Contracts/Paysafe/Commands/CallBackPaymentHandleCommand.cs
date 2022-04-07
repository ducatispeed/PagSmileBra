using MediatR;
using SfpCoreLib.Domain.Commands;
using SfpCoreLib.Domain.Validation;
using SingleApi.Svc.PaySafe.Models.Views;

namespace SingleApi.Svc.Contracts.Paysafe.Commands
{
    public class CallBackPaymentHandleCommand : CommandBase, IRequest<Result<CallBackPaymentHandleViewModel>>
    {
        public string PayloadId { get; set; }
    }
}
