using Coin_Exchange.Configuration;
using Coin_Exchange.Models;
using Coin_Exchange.Models.Modal;
using Coin_Exchange.Models.Request;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Coin_Exchange.Controllers
{
    [Route("api/payment-details")]
    [ApiController]
    public class PaymentDetailController:ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PaymentDetailController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<PaymentDetails>> addPaymentDetail([FromBody] PaymentDetailRequest req, [FromHeader(Name = "Authorization")] string jwt)
        {
            string enail = JwtProviders.GetEmailFromToken(jwt);
            User user = await _context.Users.FirstOrDefaultAsync(u => u.email == enail);
            if (user == null)
            {
                return BadRequest("user not found");
            }
            if (req.accountNumber != req.confirmAccountNumber)
            {
                return BadRequest("Account Number is not correct");
            }
            PaymentDetails paymentDetails = await _context.PaymentDetails.FirstOrDefaultAsync(c => c.user.id == user.id);
            if (paymentDetails != null)
            {
                paymentDetails.accountNumber = req.accountNumber;
                paymentDetails.accountHolderName = req.accountHolderName;
                paymentDetails.bankName = req.bankName;
                paymentDetails.ifsc = req.ifsc;
                paymentDetails.user = user;
                await _context.SaveChangesAsync();
            }
            else
            {
                paymentDetails = new PaymentDetails
                {
                    accountNumber = req.accountNumber,
                    accountHolderName = req.accountHolderName,
                    bankName = req.bankName,
                    ifsc = req.ifsc,
                    user = user
                };
                _context.PaymentDetails.Add(paymentDetails);
                await _context.SaveChangesAsync();
            }
            return Ok(paymentDetails);
        }

        [HttpGet]
        public async Task<ActionResult<PaymentDetails>> getUserPaymentDetail([FromHeader(Name = "Authorization")] string jwt)
        {
            string enail = JwtProviders.GetEmailFromToken(jwt);
            User user = await _context.Users.FirstOrDefaultAsync(u => u.email == enail);
            PaymentDetails userPaymentDetails = await _context.PaymentDetails.FirstOrDefaultAsync(p => p.user.id == user.id);
            return Ok(userPaymentDetails);
        }
    }
}
