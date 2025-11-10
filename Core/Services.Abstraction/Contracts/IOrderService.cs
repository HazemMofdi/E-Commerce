using Shared.DTOs.OrderModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstraction.Contracts
{
    public interface IOrderService
    {
        Task<OrderResult> GetOrderByIdAsync(Guid Id);
        Task<IEnumerable<OrderResult>> GetOrdersByEmailAsync(string userEmail);
        Task<OrderResult> CreateOrderAsync(string userEmail, OrderRequest orderRequest);
        Task<IEnumerable<DeliveryMethodResult>> GetDeliveryMethodsAsync();


    }
}
