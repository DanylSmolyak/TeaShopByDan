using System.Text.Json.Serialization;

namespace Core.Dtos;

public class CartItemDto
{
    public int CartId { get; set; }
    public int? Id { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public int UnitPrice { get; set; }

    public int? TotalPrice { get; set; }

    public int PriceId { get; set; }
    
    public bool? IsDeleted { get; set; }


}