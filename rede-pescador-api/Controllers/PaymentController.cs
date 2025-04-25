using Microsoft.AspNetCore.Mvc;
using rede_pescador_api.Dto;
using rede_pescador_api.Payments;
using rede_pescador_api.Models.Enums;
using rede_pescador_api.Models;
using System.Threading.Tasks;
using rede_pescador_api.Models.Enums.rede_pescador_api.Models.Enums;
using MercadoPago.Client.Payment;

namespace rede_pescador_api.Controllers
{
    [ApiController]
    [Route("rede-pescador")]
    public class PaymentController : ControllerBase
    {
        private readonly OrderService _orderService;
        private readonly MercadoPagoService _mercadoPagoService;
        private readonly PaymentService _paymentService;
        private readonly IUserRepository _userRepository;

        public PaymentController(OrderService orderService, MercadoPagoService mercadoPagoService, PaymentService paymentService)
        {
            _mercadoPagoService = mercadoPagoService;
            _orderService = orderService;
            _paymentService = paymentService;
        }

        [HttpPost("pagamentos/pix")]
        public async Task<IActionResult> CreatePixPayment([FromBody] CreatePaymentDTO dto)
        {
            var order = await _orderService.GetByIdAsync(dto.OrderId);
            if (order == null || order.Status == OrderStatus.AguardandoConfirmacao)
                return BadRequest("Pedido inválido ou já pago.");

            var buyer = await _userRepository.GetByUserIdAsync(order.BuyerId);
            if (buyer == null)
                return BadRequest("Comprador não encontrado.");

            var mercadoPagoUser = ConvertToMercadoPagoUser(buyer);

            var qrCodeData = await _mercadoPagoService.CreatePixCharge(order, mercadoPagoUser);

            var payment = new Payment
            {
                OrderId = order.Id,
                Amount = order.Total,
                Method = "PIX",
                Status = PaymentStatus.PENDENTE,
                TransactionId = qrCodeData.TransactionId
            };

            await _paymentService.SaveAsync(payment);

            return Ok(new
            {
                qrCode = qrCodeData.QrCode,
                qrCodeBase64 = qrCodeData.QrCodeBase64
            });
        }

        private MercadoPago.Resource.User.User ConvertToMercadoPagoUser(rede_pescador_api.Models.User user)
        {
            return new MercadoPago.Resource.User.User
            {
                Email = user.Email
            };
        }

        [HttpPost("pagamentos/webhook")]
        public async Task<IActionResult> PaymentWebhook([FromBody] WebhookDTO webhook)
        {
            var payment = await _paymentService.GetByTransactionIdAsync(webhook.TransactionId);
            if (payment == null)
                return NotFound();

            // Tenta converter o status do webhook para o enum PaymentStatus
            if (Enum.TryParse<PaymentStatus>(webhook.Status, true, out var status))
            {
                payment.Status = status;

                // Se o status do pagamento for "PAGO", marca o pedido como pago e atualiza o status do pedido
                await _paymentService.UpdateAsync(payment);

                if (status == PaymentStatus.PAGO)
                {
                    // Atualizar o status do pedido para "Confirmado" quando o pagamento for confirmado
                    var order = await _orderService.GetByIdAsync(payment.OrderId);
                    if (order != null && order.Status == OrderStatus.AguardandoConfirmacao)
                    {
                        // Chama o método de atualização de status do pedido
                        await _orderService.UpdateOrderStatusAsync(order.Id, OrderStatus.Confirmado);
                    }
                }
            }
            else
            {
                return BadRequest("Status de pagamento inválido.");
            }

            return Ok();
        }


    }
}
