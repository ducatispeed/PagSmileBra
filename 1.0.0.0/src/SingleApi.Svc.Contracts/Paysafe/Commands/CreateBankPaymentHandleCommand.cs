using MediatR;
using SfpCoreLib.Domain.Commands;
using SfpCoreLib.Domain.Validation;
using SingleApi.Svc.Contracts.Paysafe.Models.View;

namespace SingleApi.Svc.Contracts.Paysafe.Commands
{
    public class CreateBankPaymentHandleCommand : CommandBase, IRequest<Result<CreateBankPaymentHandleViewModel>>
    {
        public string OperationId { get; set; }
    }
}
