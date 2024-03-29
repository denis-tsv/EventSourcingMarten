using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shop.UseCases.Products.Commands;
using Shop.UseCases.Products.Dtos;
using Shop.UseCases.Products.Queries;

namespace Shop.WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductsController : ControllerBase
{
    private readonly ISender _sender;
    
    public ProductsController(ISender sender) => _sender = sender;

    [HttpPost]
    public async Task<Guid> Create(CreateProductDto dto, CancellationToken ct)
    {
        var result = await _sender.Send(new CreateProductCommand(dto), ct);
        return result;
    }
    
    [HttpGet("{id:guid}")]
    public async Task<GetProductResponseDto> Get([FromRoute]Guid id, CancellationToken ct)
    {
        var result = await _sender.Send(new GetProductQuery(id), ct);
        
        return result;
    }
}