using System.ComponentModel.DataAnnotations;

namespace Core.Entities;

public class Review : BaseEntitie
{

    public User UserServices { get; set; }

    public string UserID { get; set; }
    
    public Product Product { get; set; }

    public int ProductID { get; set; }

    [Range(1, 5)]
    public int Rating { get; set; }

    public string Comment { get; set; }
}