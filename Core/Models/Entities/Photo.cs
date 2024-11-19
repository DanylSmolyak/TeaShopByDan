using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;

namespace Core.Entities;

public class Photo : BaseEntitie
{
    public string Image { get; set; }

    public Product Product { get; set; }

    public int ProductId { get; set; } 
    
}