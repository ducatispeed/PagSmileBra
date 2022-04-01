using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SfpSharedLib.Api.RestApi;
using SingleApi.Data.Contracts.Shared;
using SingleApi.Svc.Contracts.Paysafe.Commands;
using SingleApi.Svc.Contracts.Paysafe.Models.Input;
using SingleApi.Svc.Contracts.Paysafe.Models.Requests;
using SingleApi.Svc.Contracts.Paysafe.Services;
using static SingleApi.Svc.Contracts.Paysafe.Models.Requests.PostPaymentHandleRequest;

namespace SingleApi.WebApi.Controllers
{
    
    [ApiController]
    public class PaymentController : WebApiControllerBase
    {
        private readonly IMediator _mediator;

        public PaymentController( IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreatePaymentHandle([FromBody] CreatePaymentHandleInputModel model)
        {
            return From(await _mediator.Send(new CreatePaymentHandleCommand
            {
                OperationId = model.OperationId,
                ConsumerId = model.ConsumerId,
                Country = model.Country,
                City = model.City,
                CountryCode = model.CountryCode,
                CustomerIp = model.CustomerIp,
                Zip = model.Zip,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Street = model.Street,
            }));
        }
        [Route("paymentstatus/{paymentId}")]
        [HttpGet]
        public async Task<IActionResult> GetPaymentStatus([FromRoute] string paymentId)
        {
            return From(await _mediator.Send(new GetPaymentStatusHandleCommand() { PaymentId = paymentId}));
        }
        [Route("createBankPaymentByOperationId/{operationId}")]
        [HttpPost]
        public async Task<IActionResult> CheckPaymentStatus([FromRoute]string operationId)
        {
            return From(await _mediator.Send(new CreateBankPaymentHandleCommand() { OperationId = operationId}));
        }
    }
}
