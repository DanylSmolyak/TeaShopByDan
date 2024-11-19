using Core.Entities;
using Core.Interfaces;
using Core.SpecParams;

namespace Core;

public class ProductSpecificationWithPagination : BaseSpecification<Product>
{
    public ProductSpecificationWithPagination(ProductSpecParams productParams) : base(p=>
        (string.IsNullOrEmpty(productParams.Search) || p.Name.ToLower().Contains(productParams.Search))&&
        (!productParams.CategoryId.HasValue || p.CategoryID == productParams.CategoryId)
    )
    {
        
        AddInclude(x => x.ProductPrices);
        AddInclude(x => x.TeaDetail);
        
        if (!string.IsNullOrEmpty(productParams.Sort))
        {
            switch (productParams.Sort)
            {
                case "priceAsc":
                    AddOrderBy(p => p.ProductPrices.Min(x=> x.Price));
                    break;
                case "priceDesc":
                    AddOrderByDescending(p => p.ProductPrices.Max(x => x.Price));
                    break;
                case "AverageRatingDesc":
                    AddOrderByDescending(p => p.AverageRating);
                    break;
                default:
                    AddOrderBy(p => p.Name);
                    break;
            }
        }
        
        ApplyPagination((productParams.PageIndex - 1) * productParams.PageSize, productParams.PageSize);
    }

}