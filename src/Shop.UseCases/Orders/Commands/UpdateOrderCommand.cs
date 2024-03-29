using Marten;
using MediatR;
using Shop.Domain.Entities;
using Shop.Domain.Events;
using Shop.UseCases.Exceptions;
using Shop.UseCases.Orders.Dtos;

namespace Shop.UseCases.Orders.Commands;

public record UpdateOrderCommand(Guid Id, UpdateOrderDto Dto) : IRequest;

public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand>
{
    private readonly IDocumentSession _documentSession;

    public UpdateOrderCommandHandler(IDocumentSession documentSession) => _documentSession = documentSession;

    public async Task Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await _documentSession.Events.AggregateStreamAsync<Order>(request.Id, token: cancellationToken);
        if (order == null) throw new NotFoundException(request.Id);
        
        var events = new List<object>();
        
        if (request.Dto.AddedItems != null)
        {
            events.AddRange(
                request.Dto.AddedItems
                    .Select(x => new OrderItemAdded(x.ProductId, x.Quantity))
                    .ToArray()
            );
        }
        //Update
        //Delete

        //can be interesting https://youtu.be/yWpuUHXLhYg?t=2772 WriteById and Commands https://youtu.be/yWpuUHXLhYg?t=3107
        
        _documentSession.Events.Append(request.Id, 
            request.Dto.Version + events.Count,
            events.ToArray()); // ToArray must have! 
        await _documentSession.SaveChangesAsync(cancellationToken);
    }
}