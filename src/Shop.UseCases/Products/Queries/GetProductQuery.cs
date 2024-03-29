using Marten;
using MediatR;
using Shop.Domain.Entities;
using Shop.UseCases.Exceptions;
using Shop.UseCases.Products.Dtos;

namespace Shop.UseCases.Products.Queries;

public record GetProductQuery(Guid Id): IRequest<GetProductResponseDto>;

public class GetProductQueryHandler : IRequestHandler<GetProductQuery, GetProductResponseDto>
{
    private readonly IDocumentSession _documentSession;

    public GetProductQueryHandler(IDocumentSession documentSession) => _documentSession = documentSession;

    public async Task<GetProductResponseDto> Handle(GetProductQuery request, CancellationToken cancellationToken)
    {
        var product = await _documentSession.LoadAsync<Product>(request.Id, cancellationToken);
        if (product == null) throw new NotFoundException(request.Id);

        return new GetProductResponseDto(product.Id, product.Name, product.Price);
    }
}