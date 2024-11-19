
using System.Net;
using AutoMapper;
using Core.Dtos;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;


[ApiController]
[Route("[controller]")]
public class OrderController : ControllerBase
{
    private readonly IRepository<Order> _orderRepository;
    private readonly IMapper _mapper;
    private readonly IUserService _userService;
    private readonly IOrderService _orderService;

    public OrderController(IRepository<Order> orderRepository, IMapper mapper, IUserService userService, IOrderService orderService)
    {
        _orderRepository = orderRepository;
        _mapper = mapper;
        _userService = userService;
        _orderService = orderService;
    }

    [HttpGet]
    [Authorize]
    public async Task<IReadOnlyList<OrderDto>> GetAllOrders()
    {
        var orders = await _orderRepository.ListAsync();
        return _mapper.Map<IReadOnlyList<OrderDto>>(orders);
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult<OrderDto>> GetOrderById(int id)
    {
        var order = await _orderRepository.GetbyIDAsync(id);
        if (order == null) return NotFound("Order not found");
        
        return Ok(_mapper.Map<OrderDto>(order));
    }

    [HttpGet("user")]
    [Authorize]
    public async Task<IReadOnlyList<OrderDto>> GetOrderByUserId()
    {
        var userId = GetAuthorizedUserId();
        var orders = await _orderRepository.ListAsync(x => x.UserID == userId);
        return _mapper.Map<IReadOnlyList<OrderDto>>(orders);
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<OrderDto>> AddOrder(OrderDto orderDto)
    {
        orderDto.UserID = GetAuthorizedUserId();
        var order = await _orderService.CreateOrder(orderDto);
        return Ok(_mapper.Map<OrderDto>(order));
    }

    [HttpPost("confirm/{orderId}")]
    [Authorize]
    public async Task<IActionResult> ConfirmOrder(int orderId)
    {
        try
        {
            await _orderService.ConfirmOrder(orderId);
            return Ok(new { Message = "Order confirmed successfully." });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "An error occurred while confirming the order." });
        }
    }

    private string GetAuthorizedUserId()
    {
        return User.Claims.FirstOrDefault(x => x.Type == "uid")?.Value;
    }
}

