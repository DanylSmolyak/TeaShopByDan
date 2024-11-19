namespace Core.Dtos;

public class CartDto
{
    public int Id { get; set; }
    public string? UserId { get; set; }
    public bool IsDeleted { get; set; }
    public List<CartItemDto> CartItems { get; set; }
}