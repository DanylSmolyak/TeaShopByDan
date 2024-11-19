using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities;

public class CartItem : BaseEntitie
{
    [Key]
    public Cart Cart { get; set; }
    
    public int CartId { get; set; } 
        
    public Product Product { get; set; }
    
    public int ProductID { get; set; }
    
    public int Quantity { get; set; }

    public int TotalPrice { get; set; }

    [Required]

    public int UnitPrice { get; set; }
    
}
