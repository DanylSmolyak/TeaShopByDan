namespace Core.Entities;

public class Order : BaseEntitie
{

    public User UserServices { get; set; }
    public string UserID { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime UpdatedAt { get; set; }

    public string ShippingAddress { get; set; }

    public string PhoneNumber { get; set; }

    public string Email { get; set; }

    public bool IsConfirmed { get; set; }

    public ICollection<OrderDetail>  OrdersDetails {get; set;}

}