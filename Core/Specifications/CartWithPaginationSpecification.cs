using Core.Entities;
using Core.SpecParams;

namespace Core.Specifications;

public class CartWithPaginationSpecification : BaseSpecification<Cart>
{
    public CartWithPaginationSpecification(ProductSpecParams productParams)
        : base(c => (string.IsNullOrEmpty(productParams.UserId) || c.UserID == productParams.UserId)&&
                    c.CartItems.Any(x=> !x.IsDeleted)
        ) 
            
    {
        AddInclude(c => c.CartItems);
        AddInclude(x => x.CartItems.Where(ci => !ci.IsDeleted));
        
        // Сортировка без Min и Max
        if (!string.IsNullOrEmpty(productParams.Sort))
        {
            switch (productParams.Sort)
            {
                case "priceAsc":
                    AddOrderBy(cart => cart.CartItems.Select(ci => ci.UnitPrice).Min());
                    break;
                case "priceDesc":
                    AddOrderByDescending(cart => cart.CartItems.Select(ci => ci.UnitPrice).Max());
                    break;
                case "productIdAsc":
                    AddOrderBy(cart => cart.CartItems.Select(ci => ci.ProductID).Min());
                    break;
            }
        }

        ApplyPagination((productParams.PageIndex - 1) * productParams.PageSize, productParams.PageSize);
    }
}
