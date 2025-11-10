using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.OrderModule
{
    public record OrderResult
    {
        public Guid Id { get; init; }
        public string UserEmail { get; init; } = string.Empty;
        public ShippingAddressDTO ShippingAddress { get; init; }
        public ICollection<OrderItemDTO> OrderItems { get; init; } = new List<OrderItemDTO>();
        public string PaymentStatus { get; init; } = string.Empty;
        public string DeliveryMethod { get; init; } = string.Empty;
        public int? DeliveryMethodId { get; init; }
        public decimal SubTotal { get; init; }
        public decimal Total { get; init; }
        public DateTimeOffset OrderDate { get; init; } = DateTimeOffset.UtcNow;
        public string PaymentIntentId { get; init; } = string.Empty;
    }
}
