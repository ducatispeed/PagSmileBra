using CustomLoggerLib.Crosscutting;
using GlobalEditAPI.Assets.Core.Tests.Builders;
using MediatR;
using SfpCoreLib.Domain.Validation;
using SingleApi.Data.Contracts.Shared;
using SingleApi.Svc.Contracts.Paysafe.Commands;
using SingleApi.Svc.Contracts.Paysafe.Models.View;
using SingleApi.Svc.Contracts.Paysafe.Services;
using System.Net;

namespace SingleApi.Svc.Paysafe.Handlers
{
    public class GetPaymentStatusHandleCommandHandler : IRequestHandler<GetPaymentStatusHandleCommand, Result<GetPaymentStatusHandleViewModel>>
    {
        private readonly IPaysafeApiService _paysafeApiService;
        private readonly IPartnerRepository _partnerRepository;
        private readonly IProdRepository _prodRepository;

        public GetPaymentStatusHandleCommandHandler(
            IPaysafeApiService paysafeApiService, 
            IPartnerRepository partnerRepository, 
            IProdRepository prodRepository)
        {
            _paysafeApiService = paysafeApiService.NotNull(nameof(paysafeApiService));
            _partnerRepository = partnerRepository.NotNull(nameof(partnerRepository));
            _prodRepository = prodRepository.NotNull(nameof(prodRepository));
        }

        public async Task<Result<GetPaymentStatusHandleViewModel>> Handle(GetPaymentStatusHandleCommand command, CancellationToken cancellationToken)
        {
            try
            {
                if (string.IsNullOrEmpty(command.PaymentId))
                {
                    return Result.Error<GetPaymentStatusHandleViewModel>(new Failure(HttpStatusCode.BadRequest.ToString(), "Error while hanlde payment status. Parametr paymentId must be provided"));
                }

                var config = await _partnerRepository.GetConfigurations("xxxx");

                var request = new GetPaymentRequestBuilder().WithPaymentId(command.PaymentId).Build();

                var result = await _paysafeApiService.GetPaymentStatusHandleAsync(request,config);

                if (result.Error != null)
                {
                    return Result.Error<GetPaymentStatusHandleViewModel>(new Failure(result.Error.ErrorCode.ToString(), result.Error.ErrorMessage));
                }

                return Result.Success(new GetPaymentStatusHandleViewModel
                {
                    PaymentStatus = result.Status
                });
            }
            catch (Exception ex)
            {
                return Result.Error<GetPaymentStatusHandleViewModel>(new Failure(HttpStatusCode.InternalServerError.ToString(), ex.Message));
            }
        }
    }
}
