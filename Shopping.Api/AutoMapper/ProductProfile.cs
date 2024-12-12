using AutoMapper;
using Shopping.Shared.Dtos.CRUDs;
using Shopping.Core.Entities;

namespace Shopping.Api.Automapper
{
    public class ProductProfile: Profile
    {
        public ProductProfile()
        {
            //CreateMap for both directions OR use ReverseMap() method
            //CreateMap<Product, ProductDto>().ReverseMap(); 

            CreateMap<Product, ProductDto>();
            CreateMap<ProductDto, Product>();

            CreateMap<ProductCreateDto, Product>();
            CreateMap<Product, ProductCreateDto>();

            CreateMap<ProductUpdateDto, Product>();
            CreateMap<Product, ProductUpdateDto>();

            CreateMap<CartItemDto, Product>();
            CreateMap<Product, CartItemDto>();

            CreateMap<CartItemToAddDto, Product>();
            CreateMap<Product, CartItemToAddDto>();
        }

    }
}
