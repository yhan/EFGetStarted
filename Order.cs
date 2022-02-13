public class Order
{
    public string Id { get; set; }
    public string Type { get; set; }

    public double VWAP { get; set; }

    public string? ParentOrderId { get; set; }

    //[ForeignKey("ParentOrderId")]
    public Order ParentOrder { get; set; }
    public ICollection<Order> Children { get; set; }

}