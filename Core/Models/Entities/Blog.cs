namespace Core.Entities;

public class Blog : BaseEntitie
{
    public User UserServices { get; set; }
    
    public string UserID { get; set; }

    public string Title { get; set; }

    public string Content { get; set; } 
    public ICollection<BlogComment> BlogComments { get; set; }
}