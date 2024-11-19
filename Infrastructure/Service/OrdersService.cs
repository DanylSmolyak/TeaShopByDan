using Core.Dtos;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Service;

public class OrderService : IOrderService
{
   private readonly IRepository<Product> _productRepository;
   private readonly IRepository<OrderDetail> _orderDetailRepository;
   private readonly IRepository<Order> _orderRepository;
   private readonly IRepository<CartItem> _cartItemRepository;
   private readonly IRepository<Cart> _cartRepository;
   private readonly Context _context;

   public OrderService(IRepository<Product> productRepository,
      IRepository<OrderDetail> orderDetailRepository,
      IRepository<Order> orderRepository,
      IRepository<CartItem> cartItemRepository,
      Context context,
      IRepository<Cart> cartRepository)
   {
      _orderDetailRepository = orderDetailRepository;
      _orderRepository = orderRepository;
      _productRepository = productRepository;
      _cartItemRepository = cartItemRepository;
      _cartRepository = cartRepository;
      _context = context;
   }
   public async Task<Order> CreateOrder(OrderDto orderDto)
   {

      var order = new Order
      {
         UserID = orderDto.UserID,
         CreatedAt = DateTime.UtcNow,
         Email = orderDto.Email, 
         PhoneNumber = orderDto.PhoneNumber,
         ShippingAddress = orderDto.ShippingAddress,
         OrdersDetails = new List<OrderDetail>(),
         IsConfirmed = false
      };

      order.OrdersDetails = new List<OrderDetail>();
      
      var existingCart = await _context.Cart.FirstOrDefaultAsync(x => x.UserID == orderDto.UserID);
      if (existingCart == null)
         throw new Exception("Корзина пользователя не найдена.");

      var userCartItems = await _cartItemRepository.ListAsync(x => x.CartId == existingCart.id);
      if (userCartItems == null)
         throw new Exception("Корзина пуста");

      foreach (var cartItem in userCartItems)
      {
         var orderDetail = new OrderDetail
         {
            ProductID = cartItem.ProductID,
            Quantity = cartItem.Quantity,
            UnitPrice = cartItem.UnitPrice,
            TotalPrice = cartItem.TotalPrice
         };
         order.OrdersDetails.Add(orderDetail);
      }
      
      await _orderRepository.AddAsync(order);
      return order;
   }

   public async Task ConfirmOrder(int orderId)
   {
      var order = await _orderRepository.GetbyIDAsync(orderId);

      if (order == null) throw new Exception("Order not found");
      if (order.IsConfirmed) throw new Exception("Order is already confirmed");

      foreach (var detail in order.OrdersDetails)
      {
         var product = await _productRepository.GetbyIDAsync(detail.ProductID);
         if (product == null) throw new Exception("Product not found");

         if (product.Stock < detail.Quantity)
            throw new Exception($"Not enough stock for product {product.Name}");

         product.Stock -= detail.Quantity;
         await _productRepository.EditAsync(product);
      }

      order.IsConfirmed = true;
      await _orderRepository.EditAsync(order);
   }

   public async Task<Cart> AddToCart(CartDto cartDto)
   {

      var userId = cartDto.UserId;

      var existingCart  = await _cartRepository.GetbyIDAsync(x => x.UserID == userId);
      
      if (existingCart  == null)
      {
         existingCart   = new Cart
         {
            UserID = userId,
            CartItems = new List<CartItem>()
         };
      }
      else
      {
         if (existingCart.CartItems == null)
         {
            existingCart.CartItems = new List<CartItem>();
         }
      }

      foreach (var cartItemDto in cartDto.CartItems)
      {
         var product = await _productRepository.GetbyIDAsync(cartItemDto.ProductId);
         if (product == null)
            throw new Exception("Product not found");
         
         var productPrice = await _context.ProductPrice.FirstOrDefaultAsync(p => p.ProductId == cartItemDto.ProductId && p.id == cartItemDto.PriceId);
         if (productPrice == null)
            throw new Exception("Price for product not found");

         var existingCartItem = await _context.CartItem.FirstOrDefaultAsync(x => x.ProductID == cartItemDto.ProductId && x.UnitPrice == productPrice.Price);
           

         if (existingCartItem == null)
         {
            var cartItem = new CartItem
            {
               ProductID = product.id,
               Quantity = cartItemDto.Quantity,
               UnitPrice = productPrice.Price,
               TotalPrice = cartItemDto.Quantity * productPrice.Price
            };
            
            existingCart.CartItems.Add(cartItem);
         }
         else
         {
            existingCartItem.Quantity += cartItemDto.Quantity;
            existingCartItem.TotalPrice = existingCartItem.Quantity * productPrice.Price;
         }
      }
      if (existingCart.id == 0)
      { 
         await _cartRepository.AddAsync(existingCart);
      }
      else
      {
         await _cartRepository.EditAsync(existingCart);
      }

      return existingCart;
   }

   
 

}