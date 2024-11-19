using Core.Dtos;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Infrastructure.Service;

public class ProductService
{
    private readonly IRepository<ProductPrice> _productPriceRepo;
    private readonly IRepository<Product> _productRepo;
    private readonly IRepository<TeaDetail> _teaDetailRepo;
    

    public ProductService(IRepository<ProductPrice> productPriceRepo,
        IRepository<Product> productRepo,
        IRepository<TeaDetail> teaDetailRepo)
    {
        _productPriceRepo = productPriceRepo;
        _productRepo = _productRepo;
        _teaDetailRepo = teaDetailRepo;
    }
    

}