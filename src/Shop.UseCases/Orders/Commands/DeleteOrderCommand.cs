using Marten;
using MediatR;
using Shop.Domain.Events;

namespace Shop.UseCases.Orders.Commands;

public record DeleteOrderCommand(Guid Id) : IRequest;

public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand>
{
    private readonly IDocumentSession _documentSession;

    public DeleteOrderCommandHandler(IDocumentSession documentSession) => _documentSession = documentSession;

    public async Task Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
    {
        _documentSession.Events.Append(request.Id, 1, new OrderDeleted(request.Id));
        await _documentSession.SaveChangesAsync(cancellationToken);
        
        //Optional
        _documentSession.Events.ArchiveStream(request.Id); //not the same transaction as append deleted event
        await _documentSession.SaveChangesAsync(cancellationToken);
    }
}