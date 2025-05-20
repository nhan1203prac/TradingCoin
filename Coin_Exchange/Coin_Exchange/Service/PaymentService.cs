using Coin_Exchange.Models.Modal;
using Coin_Exchange.Models.Response;
using Coin_Exchange.Models;
using Stripe.Checkout;
using Coin_Exchange.Models.Enum;
using Stripe;

namespace Coin_Exchange.Service
{
    public class PaymentService : IPaymentService
    {
        private readonly ApplicationDbContext _context;


        private const string StripeSecretKey = "sk_test_51P5IjLP1PtyodjX4QVY760xOBAmkJBWxzOWLQr4dujuFVMxWbMZkHGMZUkDwg3k4F5hiR7NCVt1SdBRiK6ix7ZB300PZx6CwkS";


        public PaymentService(ApplicationDbContext context)
        {
            _context = context;
        }
        public PaymentResponse createStripePaymentLing(User user, long amount, long orderId)
        {
            StripeConfiguration.ApiKey = StripeSecretKey;
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                Mode = "payment",
                SuccessUrl = $"http://localhost:5173/wallet?order_id={orderId}&paymentId={{CHECKOUT_SESSION_ID}}",
                CancelUrl = "http://localhost:5173/payment/cancel",
                LineItems = new List<SessionLineItemOptions>
            {
                new SessionLineItemOptions
                {
                    Quantity = 1,
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = "usd",
                        UnitAmount = amount * 100,
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = "Top up wallet"
                        }
                    }
                }
            }
            };
            try
            {
                var service = new SessionService();
                var session = service.Create(options);

                return new PaymentResponse
                {
                    paymentIntentId = session.Id,
                    payment_url = session.Url
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating Stripe session: {ex.Message}");
                return null;
            }

        }

        public bool proccedpaymentOrder(PaymentOrder paymentOrder, string paymentId)
        {
            if (paymentOrder.status == null)
            {
                paymentOrder.status = PaymentOrderStatus.PENDING;
            }

            if (paymentOrder.status == PaymentOrderStatus.PENDING)
            {
                if (paymentOrder.paymentMethod == Models.Enum.PaymentMethod.STRIPE)
                {
                    StripeConfiguration.ApiKey = StripeSecretKey;

                    try
                    {
                        var service = new SessionService();
                        var session = service.Get(paymentId);

                        var status = session.Status;
                        var amount = session.AmountTotal;

                        Console.WriteLine($"Payment Status: {status}");

                        if (status == "complete")
                        {
                            paymentOrder.status = PaymentOrderStatus.SUCCESS;

                            _context.SaveChanges();
                            return true;
                        }
                        else
                        {
                            paymentOrder.status = PaymentOrderStatus.FAILED;
                            _context.SaveChanges();
                            return false;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error retrieving payment session: {ex.Message}");
                        return false;
                    }
                }

                paymentOrder.status = PaymentOrderStatus.SUCCESS;
                _context.SaveChanges();
                return true;
            }

            return false;
        }
    }
    
    public interface IPaymentService
    {
        Boolean proccedpaymentOrder(PaymentOrder paymentOrder, String paymentId);

        PaymentResponse createStripePaymentLing(User user, long amount, long orderId);
    }
}
