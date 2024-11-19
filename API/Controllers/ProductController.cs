using System.Transactions;
using AutoMapper;
using Core;
using Core.Dtos;
using Core.Entities;
using Core.Interfaces;
using Core.SpecParams;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductController : ControllerBase
{
    private readonly IRepository<Product> _repo;
    private readonly IRepository<ProductPrice> _productPriceRepo;
    private readonly IMapper _mapper;
    private readonly IPhotoService _photoService;
    private readonly IRepository<TeaDetail> _teaDetailRepo;

    public ProductController(IRepository<Product> repo,
        IMapper mapper,
        IPhotoService photoService,
        IRepository<TeaDetail> teaDetailRepo,
        IRepository<ProductPrice> productPriceRepo)
    {
        _repo = repo;
        _mapper = mapper;
        _photoService = photoService;
        _teaDetailRepo = teaDetailRepo;
        _productPriceRepo = productPriceRepo;
    }
    [HttpGet]
    public async Task<Pagination<ProductDto>> GetProducts([FromQuery] ProductSpecParams productParams)
    {
        var totalItems = await _repo.CountAsync() + 1; 
        var spec = new ProductSpecificationWithPagination(productParams);
        var products = await _repo.FindWithSpecification(spec).ToListAsync();
        var productsDto = _mapper.Map<IReadOnlyList<ProductDto>>(products);

        foreach (var productDto in productsDto)
        {
            var photos = await _photoService.GetPhotoUrlById(productDto.id);
            productDto.Photos = photos ;
        }
        return new Pagination<ProductDto>(productParams.PageIndex, productParams.PageSize, totalItems, productsDto);
    }

    
    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDto>> GetProductById(int id)
    {
        var spec = new ProductWithDetailAndPrices(id);
        var product = await _repo.FindWithSpecification(spec).FirstOrDefaultAsync();;
        var productDto = _mapper.Map<ProductDto>(product);
        var photos = await _photoService.GetPhotoUrlById(productDto.id);
        productDto.Photos = photos ;

        return Ok(productDto);
    }
    

    [HttpPost]
    public async Task<ActionResult<ProductDto>> AddProduct([FromBody] ProductDto productDto)
    {
        using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            var product = _mapper.Map<Product>(productDto);
            await _repo.AddAsync(product);
            
            scope.Complete();
            var createdProductDto = _mapper.Map<ProductDto>(product);
            return CreatedAtAction(nameof(AddProduct), new { id = createdProductDto.id }, createdProductDto);
        }
    }


    
    
    [HttpPost("{productId}/prices")]
    public async Task<ActionResult<ProductPriceDto>> AddProductPriceController( ProductPriceDto productPriceDto, int productId)
    {
        try
        {
            var result = await AddProductPrice(productPriceDto, productId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest($"Ошибка при добавлении цены на продукт: {ex.Message}");
        }
    }


    [HttpPost("{productId}/tea-details")]
    public async Task<ActionResult<TeaDetailDto>> AddTeaDetailController( TeaDetailDto teaDetailDto, int productId)
    {
        try
        {
            var result = await AddTeaDetail(teaDetailDto, productId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest($"Ошибка при добавлении деталей чая: {ex.Message}");
        }
    }

    [HttpPost("{productId}/photo")]
    public async Task<ActionResult<List<Photo>>> AddImagesForProduct([FromForm] IFormFileCollection files , int productId)
    {
        if (files == null || files.Count == 0)
        {
            return BadRequest("File is required.");
        }
        var uploadedPhotos = new List<Photo>();
        
        foreach (var file in files)
        {
            var photo = await _photoService.UploadPhoto(file, productId);
            uploadedPhotos.Add(photo);
        }
        
        return Ok(uploadedPhotos);
    }

    private async Task<ProductPriceDto> AddProductPrice(ProductPriceDto productPriceDto, int productId)
    {
        var productPrice = _mapper.Map<ProductPrice>(productPriceDto);
        productPrice.ProductId = productId;
        await _productPriceRepo.AddAsync(productPrice);
        return _mapper.Map<ProductPriceDto>(productPrice);
    }


    private async Task<TeaDetailDto> AddTeaDetail(TeaDetailDto teaDetailDto, int prodctId)
    {
        var teaDetail = _mapper.Map<TeaDetail>(teaDetailDto);
        teaDetail.ProductId = prodctId;
        await _teaDetailRepo.AddAsync(teaDetail);
        return _mapper.Map<TeaDetailDto>(teaDetail);
    }
}
