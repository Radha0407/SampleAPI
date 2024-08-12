using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SampleAPI.Application.Command;
using SampleAPI.Application.Query;
using SampleAPI.Domain.Entities;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace SampleAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<OrdersController> _logger;
        private readonly IValidator<CreateOrderRequestCommand> _createOrderRequestValidator;

        public OrdersController(IValidator<CreateOrderRequestCommand> createOrderRequestValidator,ILogger<OrdersController> logger, IMediator mediator)
        {
            _createOrderRequestValidator = createOrderRequestValidator;
            _logger = logger;
            _mediator = mediator;
        }
        
        /// <summary>
        /// get all orders in descending order of order date
        /// </summary>
        /// <returns></returns>
        [HttpGet("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<Order>>> GetOrders()
        {
            var orders = await _mediator.Send(new GetRecentOrdersQuery());
            return Ok(orders);
        }

        /// <summary>
        /// Create a new order
        /// </summary>
        /// <param name="orderRequest"></param>
        /// <returns>adds order it in the response if successfully created</returns>
        /// <exception cref="BadHttpRequestException"></exception>
        [HttpPost("create-order")] 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateOrder( [FromBody] [Required] CreateOrderRequestCommand orderRequest)
        {
            var validationResult = await _createOrderRequestValidator.ValidateAsync(orderRequest);
            if (!validationResult.IsValid)
            {
                var errorMessage = LogValidationErrors(validationResult);
                throw new BadHttpRequestException(errorMessage);
            }
            var orderId = await _mediator.Send(orderRequest);
            return Ok(new {status = "Request has been completed" , orderId});
        }
        
        /// <summary>
        /// fetch orders for a date range by no of days provided excluding weekends
        /// </summary>
        /// <param name="days"></param>
        /// <returns></returns>
        [HttpGet("{days}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<Order>>> GetOrderByDate( [FromRoute] [Required] int days)
        {
            var orders = await _mediator.Send(new GetOrdersByDaysQuery(){ Days = days});
            return Ok(orders);
        }
        
        /// <summary>
        /// log validation errors
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        private string LogValidationErrors(ValidationResult result)
        {
            var validationErrors = string.Join("|", result.Errors
                .Select(x => x.ErrorMessage)
                .Distinct());
            var errorMessage = $"Request is not valid - {validationErrors}";
            _logger.LogError(errorMessage);
            return errorMessage;
        }
    }
}
