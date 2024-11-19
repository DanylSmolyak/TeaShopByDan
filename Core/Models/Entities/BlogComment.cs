namespace Core.Entities;

public class BlogComment : BaseEntitie
{
    
    public Blog Blog { get; set; } 

    public int BlogID { get; set; } 
    
    public User UserServices  { get; set; }

    public string UserID { get; set; }

    public string Comment { get; set; }
    
}