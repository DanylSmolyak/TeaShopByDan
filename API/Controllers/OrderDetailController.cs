using AutoMapper;
using Core.Dtos;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class OrderDetailController : ControllerBase
{
    private readonly IRepository<OrderDetail> _repository;
    private readonly IMapper _mapper;

    private OrderDetailController(IRepository<OrderDetail> repository,
        IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IReadOnlyList<OrderDetailDto>> GetAllOrderDetails()
    {
        var orderDetails = await _repository.ListAsync();
        var orderDetailsDto = _mapper.Map<IReadOnlyList<OrderDetailDto>>(orderDetails);
        return orderDetailsDto;
    }

    [HttpGet("id")]
    public async Task<OrderDetailDto> GetOrderDetailById(int id)
    {
        var orderDetail = await _repository.GetbyIDAsync(id);
        var orderDetailDto = _mapper.Map<OrderDetailDto>(orderDetail);
        return orderDetailDto;
    }

    [HttpPost]
    public async Task<OrderDetailDto> AddOrderDetail( OrderDetailDto orderDetailDto)
    {
        var orderDetail = _mapper.Map<OrderDetail>(orderDetailDto);
        await _repository.AddAsync(orderDetail);
        orderDetailDto = _mapper.Map<OrderDetailDto>(orderDetail);

        return orderDetailDto;
    }
    
    private string GetAuthorizedUserId()
    {
        var  userId= User.Claims.Where(x => x.Type == "uid").FirstOrDefault()?.Value;
        return userId;
    }
}