namespace Core.Dtos;

public class OrderDetailDto
{
    public int id { get; set; }
    public int OrderID { get; set; }
    public int ProductID { get; set; }
    public int Quantity { get; set; }
    
    public int UnitPrice { get; set; }

    public int TotalPrice { get; set; }
}