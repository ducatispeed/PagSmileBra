using CustomLoggerLib.Crosscutting;
using MediatR;
using Microsoft.Extensions.Logging;
using SfpCoreLib.Domain.Validation;
using SingleApi.Data.Contracts.Shared;
using SingleApi.Infrastructure.Constants;
using SingleApi.Svc.Contracts.Paysafe.Commands;
using SingleApi.Svc.Contracts.Paysafe.Models.Requests;
using SingleApi.Svc.Contracts.Paysafe.Models.View;
using SingleApi.Svc.Contracts.Paysafe.Services;
using static SingleApi.Svc.Contracts.Paysafe.Models.Requests.PostPaymentHandleRequest;

namespace SingleApi.Svc.Paysafe.Handlers
{
    public class CreatePaymentHandleCommandHandler : IRequestHandler<CreatePaymentHandleCommand, Result<CreatePaymentHandleViewModel>>
    {
        private readonly IPaysafeApiService _paysafeApiService;
        private readonly IPartnerRepository _partnerRepository;
        private readonly IProdRepository _prodRepository;
        private readonly ILogger<CreatePaymentHandleCommand> _logger;

        public CreatePaymentHandleCommandHandler(
            IPaysafeApiService paysafeApiService, 
            IPartnerRepository partnerRepository, 
            IProdRepository prodRepository,
            ILogger<CreatePaymentHandleCommand> logger
            )
        {
            _paysafeApiService = paysafeApiService.NotNull(nameof(paysafeApiService));
            _partnerRepository = partnerRepository.NotNull(nameof(partnerRepository));
            _prodRepository = prodRepository.NotNull(nameof(prodRepository));
            _logger = logger;
        }

        public async Task<Result<CreatePaymentHandleViewModel>> Handle(CreatePaymentHandleCommand command, CancellationToken cancellationToken)
        {
            try
            {   
                var transaction = await _partnerRepository.GetTransactionByOperationId(command.OperationId);
                var oneStepToken = await _prodRepository.GetOneStepTokenById(transaction.OneStepTokenId.ToString());
                var operationInfo = await _prodRepository.GetOperationInfo(command.OperationId);

                var merchantRefNum = Guid.NewGuid().ToString();
                var request = new PostPaymentHandleRequest
                {
                    Amount = operationInfo.Amount,//3000,
                    CurrencyCode = operationInfo.CurSymbol,//"USD",
                    PaymentType = PaymentRequest.PaymentType,
                    MerchantRefNum = merchantRefNum,
                    TransactionType = PaymentRequest.TransactionType,
                    ReturnLinks = new List<ReturnLinkModel>
                    {
                        new ReturnLinkModel
                        {
                            Rel = PaymentRequest.OnCompletedCallback,
                            Method = PaymentRequest.CallbackMethod,
                            Href = oneStepToken.TransactionOkUrl,//"https://us_commerce_site/payment/return/success"
                        },
                        new ReturnLinkModel
                        {
                            Rel = PaymentRequest.OnFailedCallback,
                            Method = PaymentRequest.CallbackMethod,
                            Href = oneStepToken.TransactionErrorUrl,//"https://us_commerce_site/payment/return/failed"
                        }
                    },


                    CustomerIp = command.CustomerIp,//"73.82.192.17",
                    RapidTransfer = new RapidTransferModel
                    {
                        ConsumerId = command.ConsumerId,//"john@gmail.com",
                        CountryCode = command.CountryCode,//"IE"
                    },
                    Profile = new ProfileModel
                    {
                        FirstName = command.FirstName,//"Ivan",
                        LastName = command.LastName//"Ivanov"
                    },
                    BillingDetails = new BillingDetailsModel
                    {
                        Street = command.Street,//"Nemiga",
                        City = command.City,//"Minsk",
                        Country = command.Country,//"GB",
                        Zip = command.Zip//"SO53 5PD"
                    }
                };

                throw new Exception("Test exception");
                var config = await _partnerRepository.GetConfigurations("xxxx");
                var response = await _paysafeApiService.PostPaymentHandleAsync(request, config);
                if(response.Error == null)
                {
                    transaction.BankPaymentReference = merchantRefNum;
                    transaction.BankOperationId = response.Id;
                    transaction.BanksPspPaymentUrl = response.Links.FirstOrDefault()?.Href ?? "";
                    await _partnerRepository.UpdateTransactionAsync(transaction);
                    await _partnerRepository.SaveTransactionPaymentReference(command.OperationId, merchantRefNum, transaction.BankId, transaction.OneStepTokenId);
                    return Result.Success<CreatePaymentHandleViewModel>(new CreatePaymentHandleViewModel
                    {
                        RedirectUrl = response.Links.FirstOrDefault()?.Href
                    });
                }
                else
                {
                    return Result.Error<CreatePaymentHandleViewModel>(new Failure(response.Error.Code, response.Error.Message));
                }

            }
            catch (Exception ex)
            {
                string message = $"Message: {ex.Message}{Environment.NewLine}StackTrace: {ex.StackTrace}{Environment.NewLine}InnerException: {ex.InnerException}";
                _logger.LogError(ex, message);
                return Result.Error<CreatePaymentHandleViewModel>(new Failure("500", "Internal server error"));
            }
        }
    }
}
