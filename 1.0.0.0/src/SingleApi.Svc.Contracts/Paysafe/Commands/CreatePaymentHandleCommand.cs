using MediatR;
using SfpCoreLib.Domain.Commands;
using SfpCoreLib.Domain.Validation;
using SingleApi.Svc.Contracts.Paysafe.Models.View;

namespace SingleApi.Svc.Contracts.Paysafe.Commands
{
    public class CreatePaymentHandleCommand : CommandBase, IRequest<Result<CreatePaymentHandleViewModel>>
    {
        public string OperationId { get; set; }
        public string CustomerIp { get; set; }
        public string ConsumerId { get; set; }
        public string CountryCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string Zip { get; set; }
        public string Country { get; set; }
    }
}
