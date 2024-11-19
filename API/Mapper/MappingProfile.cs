using AutoMapper;
using Core.Dtos;
using Core.Entities;

namespace API.Mapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Product, ProductDto>()
            .ForMember(dest => dest.CategoryID, opt => opt.MapFrom(x => x.CategoryID));
        CreateMap<ProductDto, Product>();

        CreateMap<Categorie, CategorieDto>();
        CreateMap<CategorieDto, Categorie>();
        
        CreateMap<Cart, CartDto>();
        CreateMap<CartDto, Cart>();
        
        CreateMap<CartItem, CartItemDto>();
        CreateMap<CartItemDto, CartItem>();

        CreateMap<Cart, CartItemDto>();
        CreateMap<CartItemDto, Cart>();


        CreateMap<OrderDetail, OrderDetailDto>()
            .ForMember(dest => dest.ProductID, opt => opt.MapFrom(x => x.ProductID))
            .ForMember(dest => dest.OrderID, opt => opt.MapFrom(x => x.OrderID));
        CreateMap<OrderDetailDto, OrderDetail>();

        CreateMap<Order, OrderDto>()
            .ForMember(dest => dest.UserID, opt => opt.MapFrom(x => x.UserID));
        CreateMap<OrderDto, Order>();
        
        CreateMap<Review, ReviewDto>()
            .ForMember(dest => dest.UserID, opt => opt.MapFrom(x => x.UserID))
            .ForMember(dest => dest.ProductID, opt => opt.MapFrom(x => x.ProductID));
        CreateMap<ReviewDto, Review>();
        
        
        CreateMap<ProductPrice, ProductPriceDto>();
        CreateMap<ProductPriceDto, ProductPrice>();

        CreateMap<TeaDetail, TeaDetailDto>();
        CreateMap<TeaDetailDto, TeaDetail>();
        
    }
}