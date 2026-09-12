using FluentAssertions;
using NetArchTest.Rules;
using Xunit;

namespace ArchitectureTests;

public class ModuleArchitectureTests
{
    private const string UsersDomainNamespace = "Users.Domain";
    private const string UsersApplicationNamespace = "Users.Application";
    private const string UsersInfrastructureNamespace = "Users.Infrastructure";
    private const string UsersPresentationNamespace = "Users.Presentation";

    private const string OrdersDomainNamespace = "Orders.Domain";
    private const string OrdersApplicationNamespace = "Orders.Application";
    private const string OrdersInfrastructureNamespace = "Orders.Infrastructure";
    private const string OrdersPresentationNamespace = "Orders.Presentation";

    [Fact]
    public void UsersDomain_ShouldNotHaveDependencyOnOtherLayers()
    {
        var result = Types.InAssembly(typeof(Users.Domain.User).Assembly)
            .ShouldNot()
            .HaveDependencyOnAny(
                UsersApplicationNamespace,
                UsersInfrastructureNamespace,
                UsersPresentationNamespace,
                OrdersDomainNamespace,
                OrdersApplicationNamespace,
                OrdersInfrastructureNamespace,
                OrdersPresentationNamespace)
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void OrdersDomain_ShouldNotHaveDependencyOnOtherLayers()
    {
        var result = Types.InAssembly(typeof(Orders.Domain.Order).Assembly)
            .ShouldNot()
            .HaveDependencyOnAny(
                OrdersApplicationNamespace,
                OrdersInfrastructureNamespace,
                OrdersPresentationNamespace,
                UsersDomainNamespace,
                UsersApplicationNamespace,
                UsersInfrastructureNamespace,
                UsersPresentationNamespace)
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void OrdersModule_ShouldNotAccess_UsersModuleInternalNamespaces()
    {
        // Orders should only ever access Users.Contracts, NEVER internal layers
        var result = Types.InAssemblies([
                typeof(Orders.Domain.Order).Assembly,
                typeof(Orders.Application.Orders.Commands.CreateOrder.CreateOrderCommand).Assembly,
                typeof(Orders.Infrastructure.Persistence.OrdersDbContext).Assembly,
                typeof(Orders.Presentation.Controllers.OrdersController).Assembly
            ])
            .ShouldNot()
            .HaveDependencyOnAny(
                UsersDomainNamespace,
                UsersApplicationNamespace,
                UsersInfrastructureNamespace,
                UsersPresentationNamespace)
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Handlers_ShouldHaveNameEndingWith_Handler()
    {
        var result = Types.InAssemblies([
                typeof(Users.Application.Users.Commands.RegisterUser.RegisterUserCommandHandler).Assembly,
                typeof(Orders.Application.Orders.Commands.CreateOrder.CreateOrderCommandHandler).Assembly
            ])
            .That()
            .HaveNameEndingWith("CommandHandler")
            .Or()
            .HaveNameEndingWith("QueryHandler")
            .Should()
            .HaveNameEndingWith("Handler")
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }
}
