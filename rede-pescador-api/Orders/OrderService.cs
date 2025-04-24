using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using rede_pescador_api.Dto;
using rede_pescador_api.Models.Enums;
using rede_pescador_api.Orders;
using rede_pescador_api.Repository;

public class OrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUserRepository _userRepository;
    private readonly IProductRepository _productRepository;

    public OrderService(IOrderRepository orderRepository, IUserRepository userRepository, IProductRepository productRepository)
    {
        _orderRepository = orderRepository;
        _userRepository = userRepository;
        _productRepository = productRepository;
    }

    public async Task<OrderDTO> CreateOrderAsync(OrderDTO orderDTO)
    {
     
        if (!Enum.IsDefined(typeof(DeliveryMethod), orderDTO.DeliveryMethod))
            throw new ArgumentException("Método de entrega inválido. Use 0 (Entrega) ou 1 (Retirada).");

        if (!Enum.IsDefined(typeof(OrderStatus), (int)OrderStatus.AguardandoConfirmacao)) 
            throw new ArgumentException("Status inválido para criação do pedido.");

       
        var buyer = await _userRepository.GetByUserIdAsync(orderDTO.BuyerId);
        if (buyer == null) throw new Exception("Comprador não encontrado.");

        var seller = await _userRepository.GetByUserIdAsync(orderDTO.SellerId);
        if (seller == null) throw new Exception("Vendedor não encontrado.");

        var product = await _productRepository.GetByIdAsync(orderDTO.ProductId);
        if (product == null) throw new Exception("Produto não encontrado.");

        var order = new Order
        {
            BuyerId = orderDTO.BuyerId,
            SellerId = orderDTO.SellerId,
            ProductId = orderDTO.ProductId,
            DeliveryMethod = (DeliveryMethod)orderDTO.DeliveryMethod,
            Status = OrderStatus.AguardandoConfirmacao,
            CreatedAt = DateTime.UtcNow
        };

        await _orderRepository.AddAsync(order);
        return orderDTO;
    }

    public async Task UpdateOrderStatusAsync(long orderId, OrderStatus status)
    {
        if (!Enum.IsDefined(typeof(OrderStatus), status))
            throw new ArgumentException("Status de pedido inválido. Use apenas 0 (Aguardando), 1 (Confirmado) ou 2 (Cancelado).");

        var order = await _orderRepository.GetByIdAsync(orderId);
        if (order == null)
            throw new Exception("Pedido não encontrado.");

        order.Status = status;
        await _orderRepository.UpdateAsync(order);
    }

    public async Task<IEnumerable<OrderDTO>> GetOrdersByUserAsync(long userId)
    {
        var orders = await _orderRepository.GetByUserAsync(userId);
        return orders.Select(order => new OrderDTO
        {
            Id = order.Id,
            BuyerId = order.BuyerId,
            SellerId = order.SellerId,
            ProductId = order.ProductId,
            DeliveryMethod = order.DeliveryMethod,
            Status = order.Status,
            CreatedAt = order.CreatedAt
        });
    }

    public async Task<OrderDTO> GetByIdAsync(long orderId)
    {
        var order = await _orderRepository.GetByIdAsync(orderId);
        if (order == null)
            throw new Exception("Pedido não encontrado.");

        return new OrderDTO
        {
            Id = order.Id,
            BuyerId = order.BuyerId,
            SellerId = order.SellerId,
            ProductId = order.ProductId,
            DeliveryMethod = order.DeliveryMethod,
            Status = order.Status,
            CreatedAt = order.CreatedAt
        };
    }
}
