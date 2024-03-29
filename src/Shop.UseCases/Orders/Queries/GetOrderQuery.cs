using Marten;
using MediatR;
using Shop.Domain.Entities;
using Shop.UseCases.Exceptions;
using Shop.UseCases.Orders.Dtos;

namespace Shop.UseCases.Orders.Queries;

public record GetOrderQuery(Guid Id) : IRequest<GetOrderResponseDto>;

public class GetOrderQueryHandler : IRequestHandler<GetOrderQuery, GetOrderResponseDto>
{
    private readonly IDocumentSession _documentSession;

    public GetOrderQueryHandler(IDocumentSession documentSession) => _documentSession = documentSession;

    public async Task<GetOrderResponseDto> Handle(GetOrderQuery request, CancellationToken cancellationToken)
    {
        var order = await _documentSession.Events.AggregateStreamAsync<Order>(request.Id, token: cancellationToken);
        if (order == null) throw new NotFoundException(request.Id);

        var snapshot = await _documentSession.LoadAsync<Order>(request.Id, cancellationToken);
        
        return new GetOrderResponseDto(
            order.Id,
            order.Lines.Select(x => new OrderItemDto(x.ProductId, x.Quantity)).ToArray()
        );
    }
}