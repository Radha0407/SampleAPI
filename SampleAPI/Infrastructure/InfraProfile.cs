using AutoMapper;
using SampleAPI.Domain.Entities;
using SampleAPI.Infrastructure.DAO;

namespace SampleAPI.Infrastructure;

public class InfraProfile : Profile
{
    public InfraProfile()
    {
        CreateMap<OrderDao, Order>().ReverseMap();
        CreateMap<OrderDetailsDao, Product>().ReverseMap();
    }
}