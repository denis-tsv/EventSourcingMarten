namespace Shop.UseCases.Orders.Dtos;

public record UpdateOrderDto(
    OrderItemDto[]? AddedItems,
    OrderItemDto[]? UpdatedItems,
    Guid[]? DeletedItems,
    int Version
);