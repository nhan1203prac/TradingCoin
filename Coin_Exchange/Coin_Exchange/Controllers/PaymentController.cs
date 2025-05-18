using Coin_Exchange.Configuration;
using Coin_Exchange.Models;
using Coin_Exchange.Models.Enum;
using Coin_Exchange.Models.Modal;
using Coin_Exchange.Models.Response;
using Coin_Exchange.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Coin_Exchange.Controllers
{
    public class PaymentController:ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IPaymentService _paymentService;
        public PaymentController(ApplicationDbContext context, IPaymentService paymentService)
        {
            _context = context;
            _paymentService = paymentService;


        }

        [HttpPost("payment/{paymentMethod}/amount/{amount}")]
        public async Task<ActionResult<PaymentResponse>> paymentHandler(PaymentMethod paymentMethod, long amount, [FromHeader(Name = "Authorization")] string jwt)
        {

            string enail = JwtProviders.GetEmailFromToken(jwt);
            User user = await _context.Users.FirstOrDefaultAsync(u => u.email == enail);
            PaymentOrder order = new PaymentOrder();
            order.amount = amount;
            order.paymentMethod = paymentMethod;
            order.user = user;
            order.status = PaymentOrderStatus.PENDING;
            _context.PaymentOrders.Add(order);
            await _context.SaveChangesAsync();


            PaymentResponse response = new PaymentResponse();
            if (paymentMethod.Equals(PaymentMethod.STRIPE))
            {
                response = _paymentService.createStripePaymentLing(user, amount, order.id);
            }
            return Ok(response);
        }

    } 
}
