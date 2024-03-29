using Shop.Domain.Events;

namespace Shop.Domain.Entities;

public class Order
{
    public Guid Id { get; set; }
    public int Version { get; set; }
    public List<OrderLine> Lines { get; set; } = new List<OrderLine>();

    public bool IsDeleted { get; set; }
    
    public void Apply(OrderCreated created) //IEvent<OrderCreated> also OK
    {
        Id = created.Id;
    }
    
    public void Apply(OrderItemAdded added)
    {
        Lines.Add(new OrderLine
        {
            ProductId = added.ProductId,
            Quantity = added.Quantity
        });
    }

    public void Apply(OrderDeleted deleted)
    {
        IsDeleted = true;
    }
}

public class OrderLine
{
    public Guid ProductId { get; set; }
    
    public int Quantity { get; set; }
}