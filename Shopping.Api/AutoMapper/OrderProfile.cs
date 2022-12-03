using AutoMapper;
using Shopping.Core.Entities;
using Shopping.Shared.Dtos.CRUDs;
using Shopping.Shared.Dtos;
using Shopping.Api.CQRS.Queries.OrderQuery;
using Shopping.Core.Entities.CQRSresponses;
namespace Shopping.Api.AutoMapper
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {

            CreateMap<Order, OrderResponse>()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id))

                .ForMember(d => d.UserEmail, opt => opt.MapFrom(s => s.UserEmail))
                .ForMember(d => d.UserName, opt => opt.MapFrom(s => s.UserName))

                .ForMember(d => d.OrderNumber, opt => opt.MapFrom(s => s.OrderNumber))
                .ForMember(d => d.OrderName, opt => opt.MapFrom(s => s.OrderName))
                .ForMember(d => d.OrderDate, opt => opt.MapFrom(s => s.OrderDate))               
                .ForMember(d => d.OrderItems, opt => opt.MapFrom(s => s.OrderItems))

                .ForMember(d => d.TotalCost, opt => opt.MapFrom(s => s.TotalCost))
                .ForMember(d => d.ShipToAddress, opt => opt.MapFrom(s => s.ShipToAddress));

        }

    }
}
