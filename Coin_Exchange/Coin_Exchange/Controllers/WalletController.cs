using Coin_Exchange.Configuration;
using Coin_Exchange.Models.Enum;
using Coin_Exchange.Models.Modal;
using Coin_Exchange.Models;
using Coin_Exchange.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Coin_Exchange.Models.Request;

namespace Coin_Exchange.Controllers
{
    [Route("api/wallet/")]
    [ApiController]
    public class WalletController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IPaymentService _paymentService;
        public WalletController(ApplicationDbContext context, IPaymentService paymentService)
        {
            _context = context;
            _paymentService = paymentService;
        }

        [HttpGet]
        public async Task<ActionResult<Wallet>> getWalletOfUser([FromHeader(Name = "Authorization")] string jwt)
        {

            string email = JwtProviders.GetEmailFromToken(jwt);
            User user = await _context.Users.FirstOrDefaultAsync(w => w.email == email);
            Wallet wallet = await _context.Wallets.FirstOrDefaultAsync(u => u.user.id == user.id);

            return Ok(wallet);
        }

        [HttpPut("{walletId}/transfer")]
        public async Task<ActionResult<Wallet>> walletToWalletTransfer([FromHeader(Name = "Authorization")] string jwt, long walletId, [FromBody] WalletTransactionRequest req)
        {

            string email = JwtProviders.GetEmailFromToken(jwt);
            User sender = await _context.Users.FirstOrDefaultAsync(w => w.email == email);
            Wallet walletSender = await _context.Wallets.FirstOrDefaultAsync(u => u.user.id == sender.id);
            Wallet receiverWallet = await _context.Wallets.FirstOrDefaultAsync(w => w.id == walletId);


            if (walletSender.balance.CompareTo(req.amount) < 0)
            {
                return BadRequest(new { Message = "Not enought money to tranfers" });
            }
            Decimal senderBalance = walletSender.balance - req.amount;
            walletSender.balance = senderBalance;
            await _context.SaveChangesAsync();
            receiverWallet.balance = receiverWallet.balance + req.amount;
            await _context.SaveChangesAsync();

            WalletTransaction walletTransaction = new WalletTransaction();
            walletTransaction.amount = req.amount;
            walletTransaction.wallet = walletSender;
            walletTransaction.datel = DateTime.Now;
            walletTransaction.walletTransactionType = WalletTransactionType.WALLET_TRANSFER;
            walletTransaction.purpose = req.purpose;
            walletTransaction.transferId = receiverWallet.id;
            _context.WalletTransactions.Add(walletTransaction);
            await _context.SaveChangesAsync();

            return Ok(walletSender);
        }


        [HttpPut("deposit")]
        public async Task<ActionResult<Wallet>> addBalanceToWallet([FromHeader(Name = "Authorization")] string jwt, [FromQuery(Name = "order_id")] long orderId, [FromQuery(Name = "payment_id")] string paymentId)
        {

            string email = JwtProviders.GetEmailFromToken(jwt);
            User sender = await _context.Users.FirstOrDefaultAsync(w => w.email == email);
            Wallet walletSender = await _context.Wallets.FirstOrDefaultAsync(u => u.user.id == sender.id);
            PaymentOrder paymentOrder = await _context.PaymentOrders.FirstOrDefaultAsync(c => c.id == orderId);
            if (paymentOrder == null)
            {
                return BadRequest(new { Message = "payment order not found" });
            }
            Boolean status = _paymentService.proccedpaymentOrder(paymentOrder, paymentId);
            if (status)
            {
                Decimal balance = walletSender.balance;
                Decimal newBalance = balance + paymentOrder.amount;
                walletSender.balance = newBalance;
                _context.SaveChanges();
            }

            WalletTransaction req = new WalletTransaction();

            req.amount = paymentOrder.amount;
            req.datel = DateTime.Now;
            req.purpose = "Deposit";
            req.wallet = walletSender;
            req.walletTransactionType = WalletTransactionType.ADD_MONEY;

            _context.WalletTransactions.Add(req);
            await _context.SaveChangesAsync();

            return Ok(walletSender);

        }

    }
}
