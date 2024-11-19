using AutoMapper;
using Core.Dtos;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Core.SpecParams;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class CartController : ControllerBase
{
    private readonly IRepository<Cart> _cartRepo;
    private readonly IRepository<CartItem> _cartItemRepo;
    private readonly IRepository<Product> _productItemRepo;
    private readonly IMapper _mapper;
    private readonly IUserService _userService;
    private readonly IOrderService _orderService;


    public CartController(
        IRepository<Cart> cartRepo,
        IRepository<CartItem> cartItemRepo,
        IMapper mapper,
        IUserService userService,
        IOrderService orderService
        )
    {
        _cartRepo = cartRepo;
        _cartItemRepo = cartItemRepo;
        _mapper = mapper;
        _userService = userService;
        _orderService = orderService;
    }

    [HttpGet]
    [Authorize]
    public async Task<Pagination<CartItemDto>> GetAllCartItems([FromQuery] ProductSpecParams productParams)
    {
        var totalItems = await _cartItemRepo.CountAsync(); 
        var userId = GetAuthorizedUserId();
        productParams.UserId = userId;

        var spec = new CartWithPaginationSpecification(productParams);

        var carts = await _cartRepo.FindWithSpecification(spec).ToListAsync();

        var itemsDtos =  _mapper.Map<IReadOnlyList<CartItemDto>>(carts.SelectMany(c => c.CartItems));

        return new Pagination<CartItemDto>(productParams.PageIndex, productParams.PageSize, totalItems, itemsDtos);
    }


    [HttpPost]
    [Authorize]
    public async Task<IActionResult> AddCartItem(CartDto cartDto)
    {
        var userId = GetAuthorizedUserId();
        cartDto.UserId = userId;
        var cart = await _orderService.AddToCart(cartDto);
        return Ok(_mapper.Map<CartDto>(cart));
    }
    

    [HttpPut("{cartItemId}")]
    [Authorize]
    public async Task<IActionResult> UpdateCartItem(int cartItemId, CartItemDto cartItemDto)
    {
        var cartItem = await _cartItemRepo.GetbyIDAsync(cartItemId);

        if (cartItem == null)
            return NotFound(new { Message = "Cart item not found." });

        cartItem.Quantity = cartItemDto.Quantity;
        cartItem.TotalPrice = cartItem.UnitPrice * cartItemDto.Quantity;

        await _cartItemRepo.EditAsync(cartItem);
        return Ok(_mapper.Map<CartItemDto>(cartItem));
    }
    
    [HttpPut("update-cart-items")]
    [Authorize]
    public async Task<IActionResult> UpdateCartItems(List<CartItemDto> cartItemsDto)
    {
        var cartItemsToUpdate = new List<CartItem>();

        foreach (var cartItemDto in cartItemsDto)
        {
            var cartItem = await _cartItemRepo.GetbyIDAsync(x => x.id == cartItemDto.Id);
            if (cartItem == null) continue;

            cartItem.Quantity = cartItemDto.Quantity;
            cartItem.TotalPrice = cartItem.UnitPrice * cartItemDto.Quantity;
            cartItemsToUpdate.Add(cartItem);
        }

        await _cartItemRepo.EditRangeAsync(cartItemsToUpdate); // Метод для массового обновления
        return Ok(cartItemsToUpdate.Select(item => _mapper.Map<CartItemDto>(item)));
    }

    
    [HttpDelete("{cartItemId}")]
    [Authorize]
    public async Task<IActionResult> DeleteCartItem(int cartItemId)
    {
        var cartItem = await _cartItemRepo.GetbyIDAsync(cartItemId);

        if (cartItem == null)
            return NotFound(new { Message = "Cart item not found." });

        await _cartItemRepo.DeleteAsync(cartItem); 
        return Ok(new { Message = "Cart item successfully deleted." });
    }

    
    private string GetAuthorizedUserId()
    {
        return User.Claims.FirstOrDefault(x => x.Type == "uid")?.Value;
    }
}
