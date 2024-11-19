namespace Core.Dtos;

public class OrderDto
{
    public int id { get; set; }
    
    public string? UserID { get; set; }
    
    public int? TotalAmount { get; set; }
        
    public ICollection<OrderDetailDto>? OrderDetails { get;}
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime? UpdatedAt { get; set; }
    
    public string ShippingAddress { get; set; }

    public string PhoneNumber { get; set; }
    
    public string? Email { get; set; }
    
    public bool IsConfirmed { get; set; }
    

}