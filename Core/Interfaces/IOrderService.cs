using Core.Dtos;
using Core.Entities;

namespace Core.Interfaces;

public interface IOrderService
{
    Task ConfirmOrder(int orderId);

    Task<Order> CreateOrder(OrderDto orderDto);
    Task<Cart> AddToCart(CartDto cartDto);
    
   
}