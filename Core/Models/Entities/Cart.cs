using System.ComponentModel.DataAnnotations;

namespace Core.Entities;

public class Cart : BaseEntitie
{
    public User UserServices { get; set; }

    public string UserID { get; set; }
    
    public ICollection<CartItem> CartItems { get; set; }
}