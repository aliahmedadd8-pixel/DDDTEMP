using BuildingBlocks.Application;
using BuildingBlocks.Domain;
using Orders.Contracts;
using Orders.Domain;

namespace Orders.Application.Orders.Commands.CreateOrder;

public sealed class CreateOrderCommandHandler(
    IOrderRepository orderRepository,
    ICustomerRepository customerRepository,
    IOrdersUnitOfWork unitOfWork,
    IEventBus eventBus) : ICommandHandler<CreateOrderCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var customerId = CustomerId.From(request.CustomerId);
        var customerExists = await customerRepository.ExistsAsync(customerId, cancellationToken);
        if (!customerExists)
        {
            return Result.Failure<Guid>(OrderErrors.CustomerNotFound(customerId));
        }

        var addressResult = Address.Create(request.Street, request.City, request.Country, request.ZipCode);
        if (addressResult.IsFailure)
        {
            return Result.Failure<Guid>(addressResult.Error);
        }

        var orderResult = Order.Create(customerId, addressResult.Value);
        if (orderResult.IsFailure)
        {
            return Result.Failure<Guid>(orderResult.Error);
        }

        var order = orderResult.Value;

        foreach (var itemDto in request.Items)
        {
            var moneyResult = Money.Create(itemDto.UnitPrice, itemDto.Currency);
            if (moneyResult.IsFailure)
            {
                return Result.Failure<Guid>(moneyResult.Error);
            }

            var addItemResult = order.AddItem(itemDto.ProductName, moneyResult.Value, itemDto.Quantity);
            if (addItemResult.IsFailure)
            {
                return Result.Failure<Guid>(addItemResult.Error);
            }
        }

        var completionResult = order.CompleteCreation();
        if (completionResult.IsFailure)
        {
            return Result.Failure<Guid>(completionResult.Error);
        }

        await orderRepository.AddAsync(order, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        // Publish cross-module integration event
        await eventBus.PublishAsync(
            new OrderCreatedIntegrationEvent(
                order.Id.Value,
                order.CustomerId.Value,
                order.TotalAmount.Amount,
                order.TotalAmount.Currency),
            cancellationToken);

        return Result.Success(order.Id.Value);
    }
}
