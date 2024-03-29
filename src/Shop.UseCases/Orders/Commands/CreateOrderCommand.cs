using Marten;
using MediatR;
using Shop.Domain.Entities;
using Shop.Domain.Events;
using Shop.UseCases.Orders.Dtos;

namespace Shop.UseCases.Orders.Commands;

public record CreateOrderCommand(CreateOrderDto Dto) : IRequest<Guid>;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Guid>
{
    private readonly IDocumentSession _documentSession;

    public CreateOrderCommandHandler(IDocumentSession documentSession) => _documentSession = documentSession;

    public async Task<Guid> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var id = Guid.NewGuid();
        var events = new List<object>();
        events.Add(new OrderCreated(id));
        events.AddRange(request.Dto.Items.Select(x => new OrderItemAdded(x.ProductId, x.Quantity)).ToArray());

        _documentSession.Events.StartStream<Order>(id, events);
        
        await _documentSession.SaveChangesAsync(cancellationToken);

        return id;
    }
}