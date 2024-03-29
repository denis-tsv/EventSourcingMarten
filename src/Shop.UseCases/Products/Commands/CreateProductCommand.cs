using Marten;
using MediatR;
using Shop.Domain.Entities;
using Shop.Domain.Events;
using Shop.UseCases.Products.Dtos;

namespace Shop.UseCases.Products.Commands;

public record CreateProductCommand(CreateProductDto Dto) : IRequest<Guid>;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Guid>
{
    private readonly IDocumentSession _documentSession;

    public CreateProductCommandHandler(IDocumentSession documentSession) => _documentSession = documentSession;

    public async Task<Guid> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var id = Guid.NewGuid();
        _documentSession.Events.StartStream<Product>(id, new ProductCreated(id, request.Dto.Name, request.Dto.Price));
        await _documentSession.SaveChangesAsync(cancellationToken);
        return id;
    }
}
