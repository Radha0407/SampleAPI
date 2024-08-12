using System.Collections.Generic;
using MediatR;
using SampleAPI.Domain.Entities;

namespace SampleAPI.Application.Query;

public class GetRecentOrdersQuery : IRequest<IEnumerable<Order>>
{

}