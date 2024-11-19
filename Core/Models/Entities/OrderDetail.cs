namespace Core.Entities;

public class OrderDetail : BaseEntitie
{
    public Order Order { get; set; }
    
    public int OrderID { get; set; }

    public Product Product { get; set; }
    
    public int ProductID { get; set; }

    public int Quantity { get; set; }
    public int UnitPrice { get; set; }
    
    public int TotalPrice  { get; set; }
    
}