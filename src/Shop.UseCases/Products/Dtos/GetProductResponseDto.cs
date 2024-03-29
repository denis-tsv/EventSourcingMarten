namespace Shop.UseCases.Products.Dtos;

public record GetProductResponseDto(Guid Id, string Name, decimal Price);