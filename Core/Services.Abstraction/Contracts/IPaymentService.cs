using Domain.Entities.BasketModule;
using Shared.DTOs.BasketModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstraction.Contracts
{
    public interface IPaymentService
    {
        Task<BasketDTO> CreateOrUpdatePaymentIntentAsync(string basketId);

        Task UpdatePaymentStatusAsync(string json, string signatureHeader);
    }
}
