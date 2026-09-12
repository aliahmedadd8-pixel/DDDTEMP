using BuildingBlocks.Application;
using BuildingBlocks.Domain;
using Orders.Domain;

namespace Orders.Application.Orders.Queries.GetOrderById;

public sealed class GetOrderByIdQueryHandler(IOrderRepository orderRepository)
    : IQueryHandler<GetOrderByIdQuery, OrderResponse>
{
    public async Task<Result<OrderResponse>> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var orderId = OrderId.From(request.OrderId);
        var order = await orderRepository.GetByIdAsync(orderId, cancellationToken);

        if (order is null)
        {
            return Result.Failure<OrderResponse>(OrderErrors.NotFound(orderId));
        }

        var items = order.Items.Select(item => new OrderItemResponse(
            item.Id.Value,
            item.ProductName,
            item.UnitPrice.Amount,
            item.UnitPrice.Currency,
            item.Quantity,
            item.TotalPrice.Amount)).ToList();

        var response = new OrderResponse(
            order.Id.Value,
            order.CustomerId.Value,
            order.Status.ToString(),
            order.ShippingAddress.ToString(),
            order.TotalAmount.Amount,
            order.TotalAmount.Currency,
            items,
            order.CreatedAtUtc);

        return Result.Success(response);
    }
}
