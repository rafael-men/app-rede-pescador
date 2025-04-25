using System;
using System.Threading.Tasks;
using MercadoPago.Config;
using MercadoPago.Client.Payment;
using MercadoPago.Resource.Payment;
using rede_pescador_api.Dto;
using MercadoPago.Resource.User;

namespace rede_pescador_api.Payments
{
    public class MercadoPagoService
    {

        public MercadoPagoService()
        {
            MercadoPagoConfig.AccessToken = "YOUR_ACCESS_TOKEN";
        }

        public async Task<QrCodeData> CreatePixCharge(OrderDTO order, User user)
        {

            var request = new PaymentCreateRequest
            {
                TransactionAmount = order.Total,
                Description = "Pagamento do pedido " + order.Id,
                PaymentMethodId = "pix", 
                Payer = new PaymentPayerRequest
                {
                    Email = user.Email
                }
            };

            try
            {

                var client = new PaymentClient();
                var paymentResponse = await client.CreateAsync(request);


                return new QrCodeData
                {
                    TransactionId = paymentResponse.Id.ToString(),
                    QrCode = paymentResponse.PointOfInteraction?.TransactionData?.QrCode,
                    QrCodeBase64 = paymentResponse.PointOfInteraction?.TransactionData?.QrCodeBase64
                };
            }
            catch (Exception ex)
            {
              
                throw new Exception("Erro ao criar pagamento PIX: " + ex.Message);
            }
        }
    }


    public class QrCodeData
    {
        public string TransactionId { get; set; }
        public string QrCode { get; set; }
        public string QrCodeBase64 { get; set; }
    }
}
