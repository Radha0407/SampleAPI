using AutoMapper;
using SampleAPI.Application.Command;
using SampleAPI.Domain.Entities;

namespace SampleAPI.Application;

public class ApplicationProfile : Profile 
{
    public ApplicationProfile()
    {
        CreateMap<CreateOrderRequestCommand, Order>().ReverseMap();
        CreateMap<ProductRequestCommand, Product>().ReverseMap();
    }

}