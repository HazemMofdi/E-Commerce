using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.OrderModule
{
    public record DeliveryMethodResult
    {
        public int Id { get; init; }
        public string ShortName { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public string DeliveryTime { get; init; } = string.Empty;
        public decimal Cost { get; init; }
    }
}
