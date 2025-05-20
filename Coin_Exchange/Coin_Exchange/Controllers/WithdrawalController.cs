using Coin_Exchange.Configuration;
using Coin_Exchange.Models.Enum;
using Coin_Exchange.Models.Modal;
using Coin_Exchange.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Coin_Exchange.Controllers
{
    [Route("api/")]
    [ApiController]
    public class WithdrawalController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public WithdrawalController(ApplicationDbContext context)
        {
            _context = context;
        }


        [HttpPost("withdrawal/{amount}")]
        public async Task<ActionResult<Withdrawal>> withdrawalRequest(long amount, [FromHeader(Name = "Authorization")] string jwt)
        {
            string enail = JwtProviders.GetEmailFromToken(jwt);
            User user = await _context.Users.FirstOrDefaultAsync(u => u.email == enail);
            Wallet wallet = await _context.Wallets.FirstOrDefaultAsync(w => w.user.id == user.id);

            Withdrawal withdrawal = new Withdrawal();
            withdrawal.amount = amount;
            withdrawal.user = user;
            withdrawal.status = WithdrawalStatus.PENDING;


            _context.Withdrawals.Add(withdrawal);
            await _context.SaveChangesAsync();

            if (wallet.balance > amount)
            {
                Decimal balance = wallet.balance;
                Decimal newBalance = balance - (Decimal)amount;
                wallet.balance = newBalance;
                await _context.SaveChangesAsync();
            }
            else
            {
                return BadRequest(new { Message = "Don't enought money to withdrawal" });
            }

            return Ok(withdrawal);

        }


        [HttpPatch("admin/withdrawal/{id}/proceed/{accept}")]
        public async Task<ActionResult<Withdrawal>> proceedWithdrawal(long id, Boolean accept, [FromHeader(Name = "Authorization")] string jwt)
        {
            string enail = JwtProviders.GetEmailFromToken(jwt);
            User user = await _context.Users.FirstOrDefaultAsync(u => u.email == enail);
            Withdrawal withdrawal = await _context.Withdrawals.FirstOrDefaultAsync(w => w.id == id);
            if (withdrawal == null)
            {
                throw new Exception("withdrawal not found");
            }
            withdrawal.date = DateTime.Now;
            if (accept)
            {
                withdrawal.status = WithdrawalStatus.SUCCESS;

            }
            else
            {
                withdrawal.status = WithdrawalStatus.DECLINE;
            }
            Wallet wallet = await _context.Wallets.FirstOrDefaultAsync(w => w.user.id == user.id);
            if (!accept)
            {
                Decimal balance = wallet.balance;
                Decimal newBalance = balance + withdrawal.amount;
                wallet.balance = newBalance;
                await _context.SaveChangesAsync();
            }
            else
            {
                WalletTransaction walletTransaction = new WalletTransaction
                {
                    datel = DateTime.Now,
                    walletTransactionType = WalletTransactionType.WITHDRAWAL,
                    wallet = wallet,
                    amount = withdrawal.amount,
                    purpose = "withdraw money"
                };
                _context.WalletTransactions.Add(walletTransaction);
                await _context.SaveChangesAsync();
            }
            return Ok(withdrawal);

        }



        [HttpGet("withdrawal")]
        public async Task<ActionResult<List<Withdrawal>>> getWithdrawalHistory([FromHeader(Name = "Authorization")] string jwt)
        {
            string enail = JwtProviders.GetEmailFromToken(jwt);
            User user = await _context.Users.FirstOrDefaultAsync(u => u.email == enail);
            List<Withdrawal> listWithdrawal = _context.Withdrawals.Where(w => w.user.id == user.id).ToList();
            return Ok(listWithdrawal);

        }
        [HttpGet("admin/withdrawal")]
        public async Task<ActionResult<List<Withdrawal>>> getAllWithdrawalRequest([FromHeader(Name = "Authorization")] string jwt)
        {
            string enail = JwtProviders.GetEmailFromToken(jwt);
            User user = await _context.Users.FirstOrDefaultAsync(u => u.email == enail);
            List<Withdrawal> listWithdrawal = _context.Withdrawals.Where(w => w.status == WithdrawalStatus.PENDING).ToList();
            return Ok(listWithdrawal);
        }
    }
}
