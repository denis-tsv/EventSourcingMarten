namespace Shop.UseCases.Orders.Dtos;

public record GetOrderResponseDto(
    Guid Id,
    OrderItemDto[] Items
);