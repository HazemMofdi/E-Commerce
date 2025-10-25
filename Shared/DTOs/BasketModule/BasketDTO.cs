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
        public ICollection<BasketItemDTO> BasketItems { get; init; } = [];
    }
}
