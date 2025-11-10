using AutoMapper;
using Domain.Contracts;
using Domain.Entities.OrderModule;
using Domain.Entities.ProductModule;
using Domain.Exceptions;
using Services.Abstraction.Contracts;
using Services.Specifications;
using Shared.DTOs.OrderModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implementations
{
    public class OrderService(IMapper mapper, IBasketRepository basketRepository, IUnitOfWork unitOfWork) : IOrderService
    {
        public async Task<OrderResult> CreateOrderAsync(string userEmail, OrderRequest orderRequest)
        {
            var shippingAddress = mapper.Map<ShippingAddress>(orderRequest.ShipToAddress);
            var basket = await basketRepository.GetBasketAsync(orderRequest.BasketId)
                ?? throw new NotFoundBasketException(orderRequest.BasketId);
            var orderItems = new List<OrderItem>();
            foreach (var item in basket.Items)
            {
                var product = await unitOfWork.GetRepository<Product, int>()
                    .GetByIdAsync(item.Id) ?? throw new ProductNotFoundException(item.Id);
                orderItems.Add(
                    new OrderItem(new ProductInOrderItem(product.Id, product.Name, product.PictureUrl), product.Price, item.Quantity)
                    );
            }
            var orderRepo = unitOfWork.GetRepository<Order, Guid>();
            var deliveryMethod = await unitOfWork.GetRepository<DeliveryMethod, int>()
                .GetByIdAsync(orderRequest.DeliveryMethodId) ??
                throw new DeliveryMethodNotFoundException(orderRequest.DeliveryMethodId);

            var orderExists = await orderRepo.GetByIdAsync(new OrderWithPaymentIntentIdSpecifications(basket.PaymetnIntentId));
            if(orderExists != null)
            {
                orderRepo.Delete(orderExists);
            }

            var subTotal = orderItems.Sum(o => o.Quantity * o.Price);
            var orderToCreate = new Order(userEmail, shippingAddress, orderItems, deliveryMethod, subTotal, basket.PaymetnIntentId);

            await orderRepo.AddAsync(orderToCreate);
            await unitOfWork.SaveChangesAsync();

            return mapper.Map<OrderResult>(orderToCreate);
        }



        public async Task<IEnumerable<DeliveryMethodResult>> GetDeliveryMethodsAsync()
        {
            var deliverMethods = await unitOfWork.GetRepository<DeliveryMethod, int>()
                .GetAllAsync();
            return mapper.Map<IEnumerable<DeliveryMethodResult>>(deliverMethods);
        }

        public async Task<OrderResult> GetOrderByIdAsync(Guid Id)
        {
            var order = await unitOfWork.GetRepository<Order, Guid>()
                .GetByIdAsync(new OrderWithIncludeSpecifications(Id)) ?? throw new OrderNotFoundException(Id);
            return mapper.Map<OrderResult>(order);
        }

        public async Task<IEnumerable<OrderResult>> GetOrdersByEmailAsync(string userEmail)
        {
            var orders = await unitOfWork.GetRepository<Order, Guid>()
                .GetAllAsync(new OrderWithIncludeSpecifications(userEmail));
            return mapper.Map<IEnumerable<OrderResult>>(orders);
        }
    }
}
