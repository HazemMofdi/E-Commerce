using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Abstraction.Contracts;
using Shared.DTOs.OrderModule;
using System.Security.Claims;

namespace Presentation.Controllers
{
    [Authorize]
    public class OrdersController(IServiceManager serviceManager) : ApiController
    {
        [HttpPost]
        public async Task<ActionResult<OrderResult>> CreateOrderAsync(OrderRequest orderRequest)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var order = await serviceManager.OrderService.CreateOrderAsync(userEmail, orderRequest);
            return Ok(order);
        }


        [HttpGet("{Id:guid}")]
        public async Task<ActionResult<OrderResult>> GetOrderByIdAsync(Guid Id)
        {
            var order = await serviceManager.OrderService.GetOrderByIdAsync(Id);
            return Ok(order);
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderResult>>> GetOrdersByEmailAsync()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var orders = await serviceManager.OrderService.GetOrdersByEmailAsync(userEmail);
            return Ok(orders);
        }

        [HttpGet("DeliveryMethods")]
        public async Task<ActionResult<IEnumerable<DeliveryMethodResult>>> GetDeliveryMethodsAsync()
        {
            var deliveryMethods = await serviceManager.OrderService.GetDeliveryMethodsAsync();
            return Ok(deliveryMethods);
        }
    }
}
