namespace Shop.UseCases.Orders.Dtos;

public record CreateOrderDto(
    OrderItemDto[] Items 
);