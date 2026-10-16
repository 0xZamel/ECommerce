using ECommerce.Models;
using ECommerce.Services.Interface;
using Microsoft.Extensions.Configuration;
using Stripe;
using Stripe.Checkout;


namespace ECommerce.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration _configuration;

        public PaymentService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<string> CreateCheckoutSessionAsync(
            Order order,
            List<OrderItem> orderItems)
        {
            StripeConfiguration.ApiKey =
                _configuration["Stripe:SecretKey"];

            var baseUrl = _configuration["AppSettings:BaseUrl"];

            var options = new SessionCreateOptions
            {
                Mode = "payment",

                SuccessUrl =
                    $"{baseUrl}/Customer/Cart/PaymentSuccess?session_id={{CHECKOUT_SESSION_ID}}",

                CancelUrl =
                     $"{baseUrl}/Customer/Cart/PaymentCancel",

                Metadata = new Dictionary<string, string>
                {
                    { "OrderId", order.Id.ToString() }
                },

                LineItems = new List<SessionLineItemOptions>()
                
            };

            foreach (var item in orderItems)
            {
                options.LineItems.Add(
                    new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            Currency = "usd",

                            UnitAmount = (long)(item.Price * 100),

                            ProductData =
                                new SessionLineItemPriceDataProductDataOptions
                                {
                                    Name = item.Product.Name
                                }
                        },

                        Quantity = item.Quantity
                    }
                );
            }

            var service = new SessionService();

            var session = await service.CreateAsync(options);

            return session.Url;
        }
    }
}
