using AutoMapper;
using Domain.Entities.IdentityModule;
using Domain.Entities.OrderModule;
using Shared.DTOs.OrderModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.MappingProfiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            // IdentityAddress
            CreateMap<Address, ShippingAddressDTO>().ReverseMap();

            
            CreateMap<ShippingAddress, ShippingAddressDTO>().ReverseMap();
            CreateMap<DeliveryMethod, DeliveryMethodResult>()
                .ForMember(dest => dest.Cost, options => options.MapFrom(src => src.Price));


            CreateMap<OrderItem, OrderItemDTO>()
                .ForMember(dest => dest.ProductId, options => options.MapFrom(src => src.Product.ProductId))
                .ForMember(dest => dest.ProductName, options => options.MapFrom(src => src.Product.ProductName))
                .ForMember(dest => dest.PictureUrl, options => options.MapFrom(src => src.Product.PictureUrl));


            CreateMap<Order, OrderResult>()
                .ForMember(dest => dest.PaymentStatus, options => options.MapFrom(src => src.PaymentStatus.ToString()))
                .ForMember(dest => dest.DeliveryMethod, options => options.MapFrom(src => src.DeliveryMethod.ShortName))
                .ForMember(dest => dest.Total, options => options.MapFrom(src => src.DeliveryMethod.Price + src.SubTotal));
        }
    }
}
