using AutoMapper;
using Shopping;
using Shopping.Core.Entities;
using Shopping.Core.Entities.CQRSresponses;
using Shopping.Shared;

namespace Shopping.Api.AutoMapper
{
    public class ResponseProfile : Profile
    {
        public ResponseProfile()
        {
            CreateMap<Order, OrderResponse>()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id))
                .ForMember(d => d.UserEmail, opt => opt.MapFrom(s => s.UserEmail))
                .ForMember(d => d.OrderName, opt => opt.MapFrom(s => s.OrderName))
                .ForMember(d => d.OrderDate, opt => opt.MapFrom(s => s.OrderDate))
                .ForMember(d => d.Products, opt => opt.MapFrom(s => s.OrderProducts))
                .ForMember(d => d.TotalCost, opt => opt.MapFrom(s => s.TotalCost));
        }
    }
}