using Core.Entities;

namespace Core.SpecParams;

public class ProductWithDetailAndPrices : BaseSpecification<Product>
{
    public  ProductWithDetailAndPrices(int productId) : base(x => x.id == productId)
    {
        AddInclude(x => x.ProductPrices.OrderBy(p => p.Price));
        AddInclude(x => x.TeaDetail);
    }

}