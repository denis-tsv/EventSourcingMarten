namespace Shop.UseCases.Orders.Dtos;

public record OrderItemDto(Guid ProductId, int Quantity);