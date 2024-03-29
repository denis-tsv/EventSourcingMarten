namespace Shop.Domain.Events;

public record ProductCreated(Guid Id, string Name, decimal Price);