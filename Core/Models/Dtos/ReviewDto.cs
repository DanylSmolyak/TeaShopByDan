namespace Core.Dtos;

public class ReviewDto
{
    public int id { get; set; }
    
    public string? UserID { get; set; }

    public int? ProductID { get; set; }

    public int Rating { get; set; } 
    

    public string Comment { get; set; }
}