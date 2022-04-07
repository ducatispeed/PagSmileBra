using CustomLoggerLib.Crosscutting;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SfpCoreLib.Domain.Validation;
using SingleApi.Data.Contracts.Models;
using SingleApi.Data.Contracts.Models.PROD;
using SingleApi.Data.Contracts.Shared;
using SingleApi.Infrastructure.Constants;
using SingleApi.Infrastructure.Enums;
using SingleApi.Infrastructure.Mappers;
using SingleApi.Svc.Contracts.Paysafe.Commands;
using SingleApi.Svc.Contracts.Paysafe.Models.Requests;
using SingleApi.Svc.Contracts.Paysafe.Services;
using SingleApi.Svc.PaySafe.Models.Views;

namespace SingleApi.Svc.Paysafe.Handlers
{
    public class CallBackPaymentHandler : IRequestHandler<CallBackPaymentHandleCommand, Result<CallBackPaymentHandleViewModel>>
    {
        private readonly IPaysafeApiService _paysafeApiService;
        private readonly AppSettings _appSettings;
        private readonly ICoreOperationsService _coreOperationsService;
        private readonly IProdRepository _prodRepository;
        private readonly IPartnerRepository _partnerRepository;
        private readonly ILogger<CallBackPaymentHandleCommand> _logger;

        public CallBackPaymentHandler(
            IPaysafeApiService paysafeApiService,
            IProdRepository prodRepository,
            AppSettings appSettings,
            IPartnerRepository partnerRepository,
            ICoreOperationsService coreOperationsService,
            ILogger<CallBackPaymentHandleCommand> logger
            )
            {
                _paysafeApiService = paysafeApiService.NotNull(nameof(paysafeApiService));
                _prodRepository = prodRepository.NotNull(nameof(prodRepository));
                _appSettings = appSettings.NotNull(nameof(appSettings));
                _partnerRepository = partnerRepository.NotNull(nameof(partnerRepository));
                _coreOperationsService = coreOperationsService.NotNull(nameof(coreOperationsService));  
                _logger = logger;
            }

        public async Task<Result<CallBackPaymentHandleViewModel>> Handle(CallBackPaymentHandleCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var operationId = await _partnerRepository.GetOperationIdByPayloadId(command.PayloadId);

                var config = await _partnerRepository.GetConfigurations("xxxx");
                var response = await _paysafeApiService.CallbackPaymentHandleAsync(
                    new CallbackPaymentHandleRequest() { OperationId = operationId }, config);

                if (response != null && response.Status.Equals(ErrorTypes.Ok))
                {
                    var transaction = await _partnerRepository.GetTransactionByOperationId(operationId);
                    var oneStepToken = await _prodRepository.GetOneStepTokenById(transaction.OneStepTokenId.ToString());

                    var payTransactionRequest = new PayInternalTransactionRequest
                    {
                        BankId = transaction.BankId,
                        BankReference = Guid.NewGuid().ToString(),
                        ChannelId = Channel.Online,
                        OperationId = transaction.OperationId,
                        ShopperAmount = transaction.Amount.ToString(),
                        ShopperCurrencyId = transaction.CurrencyId
                    };

                    var payTransactionResponse = await _coreOperationsService.PayTransaction(payTransactionRequest);

                    transaction.BankOperationId = payTransactionRequest.OperationId;
                    transaction.BankPaymentReference = payTransactionRequest.BankReference;
                    transaction.ChannelId = payTransactionRequest.ChannelId;
                    transaction.OneStepTokenId = oneStepToken.TokenId;
                    transaction.ProdStatus = OperationStatus.Pending;
                    transaction.IsProcessed = false;
                    transaction.BankId = payTransactionRequest.BankId;
                    transaction.IsForced = false;
                    transaction.ActualPaymentBankID = string.Empty;
                    transaction.AuthorizationCode = null;
                    transaction.BanksPspPaymentUrl = string.Empty;
                    transaction.BarCodeUrl = null;
                    transaction.PaymentDateTime = null;

                    int errorNumber = payTransactionResponse.ErrorNumber;

                    var bankStatus = BankStatus.Paid;
                    Payment payment = new Payment();
                    var message = "";
                    if (errorNumber == PayTransactionError.Success)
                    {
                        payment = await _prodRepository.GetPayment(transaction.OperationId).ConfigureAwait(false);
                        transaction.ProdStatus = OperationStatus.AlreadyPaid;
                        transaction.PaymentDateTime = payment?.CreationDateTime ?? DateTime.Now;
                        message = ErrorMapper.MapResponseCode(ErrorNumber.SuccessTransaction);
                        AddLogInformation(message, payTransactionRequest);
                    }
                    else if (errorNumber == PayTransactionError.AlreadyPaid)
                    {
                        payment = await _prodRepository.GetPayment(transaction.OperationId).ConfigureAwait(false);
                        transaction.ProdStatus = OperationStatus.AlreadyPaid;
                        transaction.IsForced = payment?.IsForced ?? false;
                        transaction.ActualPaymentBankID = payment?.BankID ?? string.Empty;
                        transaction.PaymentDateTime = payment?.CreationDateTime;
                        transaction.BankId = payTransactionRequest.BankId;
                        message = ErrorMapper.MapResponseCode(ErrorNumber.TransactionAlreadyPaid);
                        AddLog(message, payTransactionRequest);
                    }
                    else if (errorNumber == PayTransactionError.Void)
                    {
                        transaction.ProdStatus = OperationStatus.Void;
                        message = ErrorMapper.MapResponseCode(ErrorNumber.VoidTransaction);
                        AddLog(message, payTransactionRequest);
                    }
                    else if (errorNumber == PayTransactionError.Expired || errorNumber == PayTransactionError.Expired_2)
                    {
                        transaction.ProdStatus = OperationStatus.Expired;
                        message = ErrorMapper.MapResponseCode(ErrorNumber.TransactionExpired);
                        AddLog(message, payTransactionRequest);
                    }
                    else
                    {
                        transaction.ProdStatus = await _prodRepository.GetLastStatusByOperationId(transaction.OperationId);
                        message = $"Error in CoreWS: {payTransactionResponse.ErrorDescription}";
                        AddLog(message, payTransactionRequest);

                        transaction.IsProcessed = true;
                        transaction.BankStatusId = await _partnerRepository.GetBankStatusId(payTransactionRequest.BankId, (int)bankStatus);

                        await _partnerRepository.UpdateTransactionAsync(transaction);
                        //AddTransactionStatusResponseLog(transaction, message, BankStatus.Paid);
                        return Result.Success(new CallBackPaymentHandleViewModel
                        {
                            Status = payTransactionResponse.ErrorDescription
                        }); ;
                    }
                    transaction.IsProcessed = true;
                    transaction.BankStatusId = await _partnerRepository.GetBankStatusId(payTransactionRequest.BankId, (int)bankStatus);

                    await _partnerRepository.UpdateTransactionAsync(transaction);

                    //AddTransactionStatusResponseLog(transaction, bankStatus);

                }

                return Result.Success(new CallBackPaymentHandleViewModel
                {
                    Status = response.Status
                });
            }
            //if(response.Error == null)
            //{
            //    return Result.Success(new CallBackPaymentHandleViewModel{});
            //}
            //else
            //{
            //    return Result.Error<CallBackPaymentHandleViewModel>(new Failure(response.Error.Code, response.Error.Message));
            //}
            catch (Exception ex)
            {
                string message = $"Message: {ex.Message}{Environment.NewLine}StackTrace: {ex.StackTrace}{Environment.NewLine}InnerException: {ex.InnerException}";
                _logger.LogError(ex, message);
                return Result.Error<CallBackPaymentHandleViewModel>(new Failure("500", "Internal server error"));
            }
        }
        //private void AddTransactionStatusResponseLog(Transaction transaction, BankStatus bankStatus)
        //{
        //    _partnerRepository.AddTransactionStatusResponseLog(transaction, $"{_appSettings.SyAppSvcCode}: Transaction {bankStatus}", bankStatus);
        //}

        //private void AddTransactionStatusResponseLog(Transaction transaction, string description, BankStatus bankStatus)
        //{
        //    _partnerRepository.AddTransactionStatusResponseLog(transaction, $"{_appSettings.SyAppSvcCode}: {description}", bankStatus);
        //}

        private void AddLog(string message, PayInternalTransactionRequest request)
        {
            message = $@"{ message }{Environment.NewLine} Request:{ (request == null ? "null" : JsonConvert.SerializeObject(request)) }";
            _logger.LogError(message);
        }
        private void AddLogInformation(string message, PayInternalTransactionRequest request)
        {
            message = $@"{ message } Request:{ (request == null ? "null" : JsonConvert.SerializeObject(request)) }";
            _logger.LogInformation(message);
        }

        private void AddLogWithException(Exception ex, GetPaymentStatusHandleCommand command)
        {
            string message = $@"{Environment.NewLine} Command: { JsonConvert.SerializeObject(command) }";
            _logger.LogError(ex, message);
        }
    }
}