namespace Core.Entities;

public class Categorie : BaseEntitie
{
    public string CategoryName { get; set; }
    
    public ICollection<Product> Products { get; set; }
    
}