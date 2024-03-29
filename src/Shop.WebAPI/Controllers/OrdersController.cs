using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shop.UseCases.Orders.Commands;
using Shop.UseCases.Orders.Dtos;
using Shop.UseCases.Orders.Queries;

namespace Shop.WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class OrdersController : ControllerBase
{
    private readonly ISender _sender;

    public OrdersController(ISender sender) => _sender = sender;

    [HttpPost]
    public async Task<Guid> Create(CreateOrderDto dto, CancellationToken ct)
    {
        var result = await _sender.Send(new CreateOrderCommand(dto), ct);
        return result;
    }
    
    [HttpPost("{id:guid}")]
    public async Task Update([FromRoute]Guid id, UpdateOrderDto dto, CancellationToken ct)
    {
        await _sender.Send(new UpdateOrderCommand(id, dto), ct);
    }
    
    [HttpGet("{id:guid}")]
    public async Task<GetOrderResponseDto> Get([FromRoute]Guid id, CancellationToken ct)
    {
        var result = await _sender.Send(new GetOrderQuery(id), ct);
        
        return result;
    }
    
    [HttpDelete("{id:guid}")]
    public async Task Delete([FromRoute]Guid id, CancellationToken ct)
    {
        await _sender.Send(new DeleteOrderCommand(id), ct);
    }
}