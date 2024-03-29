using Shop.Domain.Events;

namespace Shop.Domain.Entities;

public class Product
{
    public Guid Id { get; set; }
    public int Version { get; set; }
    public string Name { get; set; } = default!;
    public decimal Price { get; set; }
    
    public void Apply(ProductCreated created)
    {
        Id = created.Id;
        Name = created.Name;
        Price = created.Price;
    }
}