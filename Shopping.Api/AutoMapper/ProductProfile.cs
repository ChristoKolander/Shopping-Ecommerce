using AutoMapper;
using Shopping.Api.Entities;
using Shopping.Models.Dtos.CRUDs;


namespace Shopping.Api.AutoMapper
{
    public class ProductProfile: Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductDto>();
            CreateMap<ProductDto, Product>();
            CreateMap<ProductCreateDto, Product>();
            CreateMap<Product, ProductCreateDto>();

            CreateMap<ProductUpdateDto, Product>();

        }

    }
}
