using AutoMapper;
using Domain.Contracts;
using Domain.Entities.IdentityModule;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Services.Abstraction.Contracts;
using Shared.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implementations
{
    public class ServiceManager(IUnitOfWork unitOfWork, IMapper mapper, IBasketRepository basketRepository, UserManager<ApplicationUser> userManager, IOptions<JwtOptions> options, IConfiguration configuration) : IServiceManager
    {
        private readonly Lazy<IProductService> productService = new Lazy<IProductService>(()=> new ProductService(unitOfWork, mapper));
        private readonly Lazy<IBasketService> basketService = new Lazy<IBasketService>(() => new BasketService(basketRepository, mapper));
        private readonly Lazy<IAuthenticationService> authenticationService = new Lazy<IAuthenticationService>(() => new AuthenticationService(userManager, options, mapper));
        private readonly Lazy<IOrderService> orderService = new Lazy<IOrderService>(() => new OrderService(mapper, basketRepository, unitOfWork));
        private readonly Lazy<IPaymentService> paymentService = new Lazy<IPaymentService>(() => new PaymentService(configuration, basketRepository, unitOfWork, mapper));
        public IProductService ProductService => productService.Value;
        public IBasketService BasketService => basketService.Value;
        public IAuthenticationService AuthenticationService => authenticationService.Value;
        public IOrderService OrderService => orderService.Value;
        public IPaymentService PaymentService => paymentService.Value;
    }
}
