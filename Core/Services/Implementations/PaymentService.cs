using AutoMapper;
using Domain.Contracts;
using Domain.Entities.BasketModule;
using Domain.Entities.OrderModule;
using Domain.Exceptions;
using Microsoft.Extensions.Configuration;
using Services.Abstraction.Contracts;
using Shared.DTOs.BasketModule;
using Stripe;
using Product = Domain.Entities.ProductModule.Product;
using Order = Domain.Entities.OrderModule.Order;
using Services.Specifications;
namespace Services.Implementations
{
    public class PaymentService(IConfiguration configuration,
        IBasketRepository basketRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper) : IPaymentService
    {
        //public async Task<BasketDTO> CreateOrUpdatePaymentIntentAsync(string basketId)
        //{
        //    //0] install stripe.net packedge
        //    //1] set up key ==> secret key
        //    StripeConfiguration.ApiKey = configuration.GetSection("Stripe")["SecretKey"];


        //    //2] get basket by basketId
        //    var basket = await basketRepository.GetBasketAsync(basketId)
        //        ?? throw new NotFoundBasketException(basketId);


        //    //3] validate items price ==> [basket.item.price = prodcut.price] ==> product from db
        //    foreach(var item in basket.BasketItems)
        //    {
        //        var product = await unitOfWork.GetRepository<Product, int>().GetByIdAsync(item.Id)
        //            ?? throw new ProductNotFoundException(item.Id);
        //        item.Price = product.Price;
        //    }
        //    //4] validate shipping price ==> [DeliveryMethodId] ==> shippingPrice == DeliveryMethod.Price
        //    var deliveryMethod = await unitOfWork.GetRepository<DeliveryMethod, int>().GetByIdAsync(basket.DeliveryMethodId.Value)
        //        ?? throw new DeliveryMethodNotFoundException(basket.DeliveryMethodId.Value);
        //    basket.ShippingPrice = deliveryMethod.Price;


        //    //5] Total ==> [ShippingPrice + subtotal] ==> cent ==> *100 ==> (long)
        //    //              (long) ==> ([basketItem.price.quantity * basket.item.price]   + shippingprice [DeliveryMethod.price]   *100)
        //    var amount = (long) (basket.BasketItems.Sum(i => i.Quantity * i.Price) + basket.ShippingPrice) * 100;


        //    //6] Create or Update paymentIntentId
        //    var stripeService = new PaymentIntentService();
        //    if(string.IsNullOrEmpty(basket.PaymetnIntentId))
        //    {
        //        var options = new PaymentIntentCreateOptions()
        //        {
        //            Amount = amount,
        //            PaymentMethodTypes = ["card"],
        //            Currency = "USD"
        //        };
        //        var paymentIntent = await stripeService.CreateAsync(options);
        //        basket.PaymetnIntentId = paymentIntent.Id;
        //        basket.ClientSecret = paymentIntent.ClientSecret;
        //    }
        //    else
        //    {
        //        var options = new PaymentIntentUpdateOptions()
        //        {
        //            Amount = amount
        //        };
        //        await stripeService.UpdateAsync(basket.PaymetnIntentId, options);
        //    }


        //    //7] save changes [Update] basket
        //    await basketRepository.CreateOrUpdateBasketAsync(basket);


        //    //8] map to basketDTO ==> return
        //    return mapper.Map<BasketDTO>(basket);
        //}






        public async Task<BasketDTO> CreateOrUpdatePaymentIntentAsync(string basketId)
        {
            
            StripeConfiguration.ApiKey = configuration.GetSection("Stripe")["SecretKey"];
            var basket = await GetBasketAsync(basketId);
            await ValidateBasketAsync(basket);
            var amount = CalculateTotal(basket);
            await CreateOrUpdatePaymentIntentOptionsAsync(amount, basket);
            await CreateOrUpdateBasketAsync(basket);
            return mapper.Map<BasketDTO>(basket);
        }

        

        private async Task<CustomerBasket> GetBasketAsync(string basketId)
        {
            return await basketRepository.GetBasketAsync(basketId)
                ?? throw new NotFoundBasketException(basketId);
        }

        private async Task ValidateBasketAsync(CustomerBasket basket)
        {
            //3] validate items price ==> [basket.item.price = prodcut.price] ==> product from db
            foreach (var item in basket.Items)
            {
                var product = await unitOfWork.GetRepository<Product, int>().GetByIdAsync(item.Id)
                    ?? throw new ProductNotFoundException(item.Id);
                item.Price = product.Price;
            }
            //4] validate shipping price ==> [DeliveryMethodId] ==> shippingPrice == DeliveryMethod.Price
            var deliveryMethod = await unitOfWork.GetRepository<DeliveryMethod, int>().GetByIdAsync(basket.DeliveryMethodId.Value)
                ?? throw new DeliveryMethodNotFoundException(basket.DeliveryMethodId.Value);
            basket.ShippingPrice = deliveryMethod.Price;
        }

        private long CalculateTotal(CustomerBasket basket)
        {
            //5] Total ==> [ShippingPrice + subtotal] ==> cent ==> *100 ==> (long)
            //              (long) ==> ([basketItem.price.quantity * basket.item.price]   + shippingprice [DeliveryMethod.price]   *100)
            var amount = (long)(basket.Items.Sum(i => i.Quantity * i.Price) + basket.ShippingPrice) * 100;
            return amount;
        }

        private async Task CreateOrUpdatePaymentIntentOptionsAsync(long amount, CustomerBasket basket)
        {
            var stripeService = new PaymentIntentService();
            if (string.IsNullOrEmpty(basket.PaymetnIntentId))
            {
                var options = new PaymentIntentCreateOptions()
                {
                    Amount = amount,
                    PaymentMethodTypes = ["card"],
                    Currency = "USD"
                };
                var paymentIntent = await stripeService.CreateAsync(options);
                basket.PaymetnIntentId = paymentIntent.Id;
                basket.ClientSecret = paymentIntent.ClientSecret;
            }
            else
            {
                var options = new PaymentIntentUpdateOptions()
                {
                    Amount = amount
                };
                await stripeService.UpdateAsync(basket.PaymetnIntentId, options);
            }
        }

        private async Task CreateOrUpdateBasketAsync(CustomerBasket basket)
        {
            await basketRepository.CreateOrUpdateBasketAsync(basket);
        }

        public async Task UpdatePaymentStatusAsync(string json, string signatureHeader)
        {
            string endpointSecret = configuration.GetSection("Stripe")["EndPointSecret"];
          
            var stripeEvent = EventUtility.ParseEvent(json, throwOnApiVersionMismatch: false);

            stripeEvent = EventUtility.ConstructEvent(json, signatureHeader, endpointSecret, throwOnApiVersionMismatch: false);
            var paymentIntent = stripeEvent.Data.Object as PaymentIntent;

            if (stripeEvent.Type == EventTypes.PaymentIntentSucceeded)
            {
                await UpdatePaymentStatusRecievedAsync(paymentIntent.Id);
            }
            else if (stripeEvent.Type == EventTypes.PaymentIntentPaymentFailed)
            {
                await UpdatePaymentStatusFailedAsync(paymentIntent.Id);
            }
            else
            {
                // Unexpected event type
                Console.WriteLine("Unhandled event type: {0}", stripeEvent.Type);
            }
        }

        private async Task UpdatePaymentStatusRecievedAsync(string paymentIntentId)
        {
            var orderRepo = unitOfWork.GetRepository<Order, Guid>();
            var order = await orderRepo.GetByIdAsync(new OrderWithPaymentIntentIdSpecifications(paymentIntentId));

            if(order is not null)
            {
                order.PaymentStatus = OrderPaymentStatus.PaymentRecieved;
                orderRepo.Update(order);
                await unitOfWork.SaveChangesAsync();
            }
        }
        private async Task UpdatePaymentStatusFailedAsync(string paymentIntentId)
        {
            var orderRepo = unitOfWork.GetRepository<Order, Guid>();
            var order = await orderRepo.GetByIdAsync(new OrderWithPaymentIntentIdSpecifications(paymentIntentId));

            if (order is not null)
            {
                order.PaymentStatus = OrderPaymentStatus.PaymentFailed;
                orderRepo.Update(order);
                await unitOfWork.SaveChangesAsync();
            }
        }
        
    }
}
