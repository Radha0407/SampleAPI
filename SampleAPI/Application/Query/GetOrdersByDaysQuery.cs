using System.Collections.Generic;
using MediatR;
using SampleAPI.Domain.Entities;

namespace SampleAPI.Application.Query;

public class GetOrdersByDaysQuery : IRequest<IEnumerable<Order>>
{
    public int Days { get; set; }
}