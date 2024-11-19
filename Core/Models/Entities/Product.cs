using System.ComponentModel;

namespace Core.Entities;

public class Product : BaseEntitie
{
    
    public string Name  { get; set; }
    
    public string Description { get; set; }
    
    public int Stock { get; set; }
    
    public double AverageRating { get; set; }

    public Categorie Categorie { get; set; }
    
    public int? CategoryID { get; set; } 
    
    public ICollection<OrderDetail>  OrdersDetails {get; set;}
    public ICollection<ProductPrice>  ProductPrices {get; set;}
    
    public ICollection<CartItem> CartItems { get; set; } 
    
    public TeaDetail? TeaDetail { get; set; }
    
    public ICollection<Review>  Reviews {get; set;}
    public ICollection<Photo>  Photos {get; set;}
    


}
