namespace Core.Dtos;

public class BlogDto
{
    public int Id { get; set; } 
    public string Title { get; set; }
    public string Content { get; set; } 
    public string UserID { get; set; }  
    public ICollection<BlogCommentDto> BlogComments { get; set; }  
}