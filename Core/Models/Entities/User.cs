using Core.Dtos;
using Microsoft.AspNetCore.Identity;

namespace Core.Entities;

public class User : IdentityUser
{

    public string? Image { get; set; }
    public string Address { get; set; }
    
    public ICollection<Order> Orders { get; set; }
    
    public ICollection<Review>  Reviews {get; set;}
    
    public ICollection<Blog>  Blogs {get; set;}
    
    public ICollection<BlogComment>  BlogComments {get; set;}
    
    public ICollection<Cart>  Carts {get; set;}
    
    
    public List<RefreshToken> RefreshTokens { get; set; }
    
}