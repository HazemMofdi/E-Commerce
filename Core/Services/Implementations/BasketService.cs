using AutoMapper;
using Domain.Contracts;
using Domain.Entities.BasketModule;
using Domain.Exceptions;
using Services.Abstraction.Contracts;
using Shared.DTOs.BasketModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implementations
{
    public class BasketService(IBasketRepository basketRepository, IMapper mapper) : IBasketService
    {
        public async Task<BasketDTO> CreateOrUpdateBasketAsync(BasketDTO basketDTO)
        {
            var basket = mapper.Map<CustomerBasket>(basketDTO);
            var createdOrUpdatedBasket = await basketRepository.CreateOrUpdateBasketAsync(basket);
            return createdOrUpdatedBasket is null ? throw new Exception("Basket Cant Be Created Or Updated")
                : mapper.Map<BasketDTO>(createdOrUpdatedBasket);
        }

        public async Task<bool> DeleteBasketAsync(string Id)
        {
            return await basketRepository.DeleteBasketAsync(Id);
        }

        public async Task<BasketDTO> GetBasketAsync(string Id)
        {
            var basket = await basketRepository.GetBasketAsync(Id);
            return basket is null ? throw new NotFoundBasketException(Id) :
                mapper.Map<BasketDTO>(basket);

        }
    }
}
