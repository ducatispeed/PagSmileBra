using CustomLoggerLib.Crosscutting;
using GlobalEditAPI.Assets.Core.Tests.Builders;
using MediatR;
using SfpCoreLib.Domain.Validation;
using SingleApi.Data.Contracts.Shared;
using SingleApi.Infrastructure.Enums;
using SingleApi.Svc.Contracts.Paysafe.Commands;
using SingleApi.Svc.Contracts.Paysafe.Models.Requests;
using SingleApi.Svc.Contracts.Paysafe.Models.Responses;
using SingleApi.Svc.Contracts.Paysafe.Models.View;
using SingleApi.Svc.Contracts.Paysafe.Services;
using System.Net;

namespace SingleApi.Svc.Paysafe.Handlers
{
    public class CreateBankPaymentHandleCommandHandler : IRequestHandler<CreateBankPaymentHandleCommand, Result<CreateBankPaymentHandleViewModel>>
    {
        private readonly IPaysafeApiService _paysafeApiService;
        private readonly IPartnerRepository _partnerRepository;
        private readonly IProdRepository _prodRepository;

        public CreateBankPaymentHandleCommandHandler(IPaysafeApiService paysafeApiService, IPartnerRepository partnerRepository, IProdRepository prodRepository)
        {
            _paysafeApiService = paysafeApiService.NotNull(nameof(paysafeApiService));
            _partnerRepository = partnerRepository.NotNull(nameof(partnerRepository));
            _prodRepository = prodRepository.NotNull(nameof(prodRepository));
        }

        public async Task<Result<CreateBankPaymentHandleViewModel>> Handle(CreateBankPaymentHandleCommand command, CancellationToken cancellationToken)
        {
            try
            {
                if (string.IsNullOrEmpty(command.OperationId))
                {
                    return Result.Error<CreateBankPaymentHandleViewModel>(new Failure(HttpStatusCode.BadRequest.ToString(), "Error while checking payment status. OperationId must be provided"));
                }
                var operationInfo = await _prodRepository.GetOperationInfo(command.OperationId);

                var transaction = await _partnerRepository.GetTransactionByOperationId(command.OperationId);
                if (transaction == null)
                {
                    return Result.Error<CreateBankPaymentHandleViewModel>(new Failure(HttpStatusCode.InternalServerError.ToString(), "Error while getting transaction. Transaction not found"));
                }

                var oneStepToken = await _prodRepository.GetOneStepTokenById(transaction.OneStepTokenId.ToString());
                var config = await _partnerRepository.GetConfigurations("xxxx");

                if (operationInfo == null)
                {
                    return Result.Error<CreateBankPaymentHandleViewModel>(new Failure(HttpStatusCode.InternalServerError.ToString(), "Error while getting operation. Operation not found"));
                }
                if (!config.Any())
                {
                    return Result.Error<CreateBankPaymentHandleViewModel>(new Failure(HttpStatusCode.InternalServerError.ToString(), "Error while getting configuration. Configuration not found"));
                }

                var request = new GetPaymentRequestBuilder().WithTestValues(operationInfo,oneStepToken).Build();
                var response = await _paysafeApiService.GetPaymentStatusHandleAsync(request, config);

                if (response.Error != null)
                {
                    return Result.Error<CreateBankPaymentHandleViewModel>(new Failure(response.Error.ErrorCode.ToString(), response.Error.ErrorMessage));
                }
                if (response.Status.Equals(nameof(PaymentStatusTypes.PAYABLE)))
                {
                    var paymentHubRequest = new PostPaymentHubHandleRequest();
                    paymentHubRequest.MerchantRefNum = transaction.BankPaymentReference;
                    paymentHubRequest.Amount = operationInfo.Amount;
                    paymentHubRequest.CurrencyCode = operationInfo.CurSymbol;
                    paymentHubRequest.Description = "";// to do
                    paymentHubRequest.CustomerIp = "";// to do
                    var configPaymentHubRequest = await _partnerRepository.GetConfigurations("paymentHubRequestsettings");

                    var paymentHubResponse = await _paysafeApiService.PostPaymentHubHandleAsync(paymentHubRequest, configPaymentHubRequest);

                        if(paymentHubResponse.Status != null)
                        {
                            return Result.Success(new CreateBankPaymentHandleViewModel(){});
                        }
                      return Result.Error<CreateBankPaymentHandleViewModel>(new Failure(HttpStatusCode.InternalServerError.ToString(), $"Error. PaymentHub returned {paymentHubResponse.Status}"));
                }
                else
                {
                      return Result.Error<CreateBankPaymentHandleViewModel>(
                          new Failure(HttpStatusCode.NotImplemented.ToString(),
                          $"Error occured while creating payment by operationid {command.OperationId} with status {response.Status}"));
                }
            }
            catch (Exception ex)
            {
                return Result.Error<CreateBankPaymentHandleViewModel>(new Failure(HttpStatusCode.InternalServerError.ToString(), ex.Message));
            }
        }
    }
}
