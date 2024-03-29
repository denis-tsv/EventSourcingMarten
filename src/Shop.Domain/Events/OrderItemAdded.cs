namespace Shop.Domain.Events;

public record OrderItemAdded(Guid ProductId, int Quantity);