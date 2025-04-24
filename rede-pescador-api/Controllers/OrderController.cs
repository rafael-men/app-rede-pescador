using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using rede_pescador_api.Dto;
using rede_pescador_api.Models.Enums;

namespace rede_pescador_api.Controllers
{
    [Route("rede-pescador/pedidos")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly OrderService _orderService;

        public OrderController(OrderService orderService)
        {
            _orderService = orderService;
        }

        [Authorize(Roles = "ESTABELECIMENTO")]
        [HttpPost("novo")]
        public async Task<IActionResult> CreateOrderAsync([FromBody] OrderDTO orderDTO)
        {
            var createdOrder = await _orderService.CreateOrderAsync(orderDTO);
            return CreatedAtAction(nameof(GetOrderByIdAsync), new { id = createdOrder.Id }, createdOrder);
        }

        [Authorize(Roles = "ESTABELECIMENTO")]
        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateOrderStatusAsync(long id, [FromBody] OrderStatus status)
        {
            await _orderService.UpdateOrderStatusAsync(id, status);
            return NoContent();
        }

        [Authorize(Roles = "ESTABELECIMENTO")]
        [HttpGet("usuario/{id}")]
        public async Task<IActionResult> GetOrdersByUserAsync(long id)
        {
            var orders = await _orderService.GetOrdersByUserAsync(id);
            return Ok(orders);
        }

        [Authorize(Roles = "ESTABELECIMENTO")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderByIdAsync(long orderId)
        {
            var order = await _orderService.GetByIdAsync(orderId);
            if (order == null) return NotFound();
            return Ok(order);
        }
    }
}
