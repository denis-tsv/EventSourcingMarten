using Marten;
using Marten.Events;
using Marten.Events.Aggregation;
using Shop.Domain.Events;

namespace Shop.UseCases.Orders.Projections;

public class OrderInfo
{
    public Guid Id { get; set; }
    public int Version { get; set; }
    public List<OrderItemInfo> Items { get; set; } = null!;
    public bool IsDeleted { get; set; }
}

public class OrderItemInfo
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}

public class OrderInfoProjection : SingleStreamProjection<OrderInfo>
{
    public OrderInfo Create(OrderCreated created)
    {
        return new OrderInfo
        {
            Id = created.Id,
            Items = new List<OrderItemInfo>()
        };
    }

    public void Apply(OrderItemAdded added, OrderInfo order)
    {
        order.Items.Add(new OrderItemInfo
        {
            ProductId = added.ProductId, 
            Quantity = added.Quantity
        });
    }

    public void Apply(IEvent<OrderDeleted> evt, OrderInfo order, IQuerySession session)
    {
        order.IsDeleted = true;
    }
}