using Microsoft.AspNetCore.Mvc;
using Services.Abstraction.Contracts;
using Shared.DTOs.BasketModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    public class PaymentsController(IServiceManager serviceManager) : ApiController
    {
        [HttpPost("{basketId}")]
        public async Task<ActionResult<BasketDTO>> CreateOrUpdatePaymentIntentIdAsync(string basketId)
        {
            return Ok(await serviceManager.PaymentService.CreateOrUpdatePaymentIntentAsync(basketId));
        }

        [HttpPost("Webhook")]
        public async Task<IActionResult> WebHook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            var signatureHeader = Request.Headers["Stripe-Signature"];
            await serviceManager.PaymentService.UpdatePaymentStatusAsync(json, signatureHeader);
            return new EmptyResult();
        }
    }
}
