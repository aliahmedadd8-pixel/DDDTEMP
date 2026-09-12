using BuildingBlocks.Presentation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Orders.Application.Orders.Commands.CancelOrder;
using Orders.Application.Orders.Commands.CreateOrder;
using Orders.Application.Orders.Queries.GetOrderById;
using Orders.Presentation.Contracts;

namespace Orders.Presentation.Controllers;

[Route("api/orders")]
public sealed class OrdersController(ISender sender) : ApiController
{
    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreateOrder(
        [FromBody] CreateOrderRequest request,
        CancellationToken cancellationToken)
    {
        var items = request.Items.Select(i => new CreateOrderItemDto(
            i.ProductName,
            i.UnitPrice,
            i.Currency,
            i.Quantity)).ToList();

        var command = new CreateOrderCommand(
            request.CustomerId,
            request.Street,
            request.City,
            request.Country,
            request.ZipCode,
            items);

        var result = await sender.Send(command, cancellationToken);
        return HandleResult(result);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(OrderResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        var query = new GetOrderByIdQuery(id);
        var result = await sender.Send(query, cancellationToken);
        return HandleResult(result);
    }

    [HttpPut("{id:guid}/cancel")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Cancel(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        var command = new CancelOrderCommand(id);
        var result = await sender.Send(command, cancellationToken);
        return HandleResult(result);
    }
}
