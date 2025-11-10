using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.BasketModule
{
    public record BasketDTO
    {
        public string Id { get; init; }
        public ICollection<BasketItemDTO> Items { get; init; } = [];
        public string? PaymetnIntentId { get; init; }
        public string? ClientSecret { get; init; }
        public decimal? ShippingPrice { get; init; }
        public int? DeliveryMethodId { get; init; }
    }
}
