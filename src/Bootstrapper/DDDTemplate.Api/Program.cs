using BuildingBlocks.Application;
using BuildingBlocks.Infrastructure;
using BuildingBlocks.Presentation;
using DDDTemplate.Api.Services;
using Orders.Infrastructure;
using Orders.Infrastructure.Persistence;
using Orders.Presentation;
using Orders.Presentation.Controllers;
using Serilog;
using Users.Infrastructure;
using Users.Infrastructure.Persistence;
using Users.Presentation;
using Users.Presentation.Controllers;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

// Core services
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUser, CurrentUser>();

// BuildingBlocks
builder.Services.AddBuildingBlocksInfrastructure();
builder.Services.AddBuildingBlocksPresentation();

// Modules Registration
builder.Services.AddUsersPresentation();
builder.Services.AddUsersInfrastructure(builder.Configuration);

builder.Services.AddOrdersPresentation();
builder.Services.AddOrdersInfrastructure(builder.Configuration);

// Add Controllers from all modules
builder.Services.AddControllers()
    .AddApplicationPart(typeof(UsersController).Assembly)
    .AddApplicationPart(typeof(OrdersController).Assembly);

// OpenAPI / Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHealthChecks();

var app = builder.Build();

// Migrate/Ensure database created on startup for development
using (var scope = app.Services.CreateScope())
{
    var usersDbContext = scope.ServiceProvider.GetRequiredService<UsersDbContext>();
    await usersDbContext.Database.EnsureCreatedAsync();

    var ordersDbContext = scope.ServiceProvider.GetRequiredService<OrdersDbContext>();
    await ordersDbContext.Database.EnsureCreatedAsync();
}

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Modular Monolith API v1");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/health");

app.Run();

// Required for Integration Testing
public partial class Program;
