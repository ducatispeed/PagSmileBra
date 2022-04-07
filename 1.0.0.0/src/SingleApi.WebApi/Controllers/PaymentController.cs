using MediatR;
using Microsoft.AspNetCore.Mvc;
using SfpSharedLib.Api.RestApi;
using SingleApi.Svc.Contracts.Paysafe.Commands;

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
        [Route("callbackProcess")]
        [HttpPost]
        public async Task<IActionResult> CallbackPayment([FromRoute] string payloadId)
        {
            return From(await _mediator.Send(new CallBackPaymentHandleCommand()
            {
                PayloadId = payloadId
                //PayloadId = "1191168044058050906"
            })); ;
        }
    }
}
