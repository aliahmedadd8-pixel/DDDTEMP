using BuildingBlocks.Application;

namespace Orders.Application.Orders.Commands.CancelOrder;

public sealed record CancelOrderCommand(Guid OrderId) : ICommand;
