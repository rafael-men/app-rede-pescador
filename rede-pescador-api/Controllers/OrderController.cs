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

        [Authorize(Roles = "PESCADOR")]
        [HttpPost("novo")]
        public async Task<IActionResult> CreateOrderAsync([FromBody] OrderDTO orderDTO)
        {
            await _orderService.CreateOrderAsync(orderDTO);
            return Ok("Pedido Criado");
        }

        [Authorize(Roles = "PESCADOR")]
        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateOrderStatusAsync(long id, [FromBody] OrderStatus status)
        {
            await _orderService.UpdateOrderStatusAsync(id, status);
            return NoContent();
        }

        [Authorize(Roles = "PESCADOR")]
        [HttpGet("usuario/{id}")]
        public async Task<IActionResult> GetOrdersByUserAsync(long id)
        {
            var orders = await _orderService.GetOrdersByUserAsync(id);
            return Ok(orders);
        }

        [Authorize(Roles = "PESCADOR")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderByIdAsync(long id)
        {
            var order = await _orderService.GetByIdAsync(id);
            if (order == null) return NotFound();
            return Ok(order);
        }

    }
}
