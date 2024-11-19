using System.ComponentModel;
using Core.Entities;

namespace Core.Dtos;

public class ProductDto
{
    public int id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int Stock { get; set; }
    [DefaultValue(null)]
    public int? CategoryID { get; set; }

    public ICollection<ProductPriceDto> ProductPrices { get; set; }
    
    public ICollection<PhotoDto>? Photos { get; set; }
    
    public TeaDetailDto? TeaDetail { get; set; }

}