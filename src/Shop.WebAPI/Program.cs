using Marten;
using Marten.Events.Daemon.Resiliency;
using Marten.Events.Projections;
using Shop.Domain.Entities;
using Shop.Domain.Events;
using Shop.UseCases.Orders.Commands;
using Shop.UseCases.Orders.Projections;
using Shop.WebAPI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMarten(opt =>
{
    opt.Connection("User ID=postgres;Password=postgres;Host=localhost;Port=5432;Database=ShopES");
    opt.DatabaseSchemaName = "events";

    opt.Events.AddEventType<OrderCreated>();
    opt.Events.AddEventType<OrderItemAdded>();
    opt.Events.AddEventType<OrderDeleted>();

    opt.Projections.Snapshot<Order>(SnapshotLifecycle.Inline);
    
    opt.Events.AddEventType<ProductCreated>();
    opt.Projections.Snapshot<Product>(SnapshotLifecycle.Async);
   
    opt.Projections.Add<OrderInfoProjection>(ProjectionLifecycle.Async);
    
})
    .UseLightweightSessions()
    .AddAsyncDaemon(DaemonMode.Solo)
    ;

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<CreateOrderCommand>());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<ExceptionMiddleware>();

app.MapControllers();

app.Run();