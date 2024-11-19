namespace Core.Entities;

public class ProductPrice : BaseEntitie
{
    public int Price { get; set; }

    public int WeightGrams { get; set; }

    public Product Product { get; set; }

    public int ProductId { get; set; }
}