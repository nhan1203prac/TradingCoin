using Coin_Exchange.Configuration;
using Coin_Exchange.Models;
using Coin_Exchange.Models.Modal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Coin_Exchange.Controllers
{
    [Route("api/")]
    [ApiController]
    public class WalletTransationController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public WalletTransationController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("transaction")]
        public async Task<ActionResult<List<WalletTransaction>>> getAllTransactionWallet([FromHeader(Name = "Authorization")] string jwt)
        {
            string enail = JwtProviders.GetEmailFromToken(jwt);
            User user = await _context.Users.FirstOrDefaultAsync(u => u.email == enail);
            Wallet wallet = await _context.Wallets.FirstOrDefaultAsync(w => w.user.id == user.id);
            List<WalletTransaction> result = _context.WalletTransactions.Where(w => w.wallet.id == wallet.id).ToList();
            return Ok(result);

        }
    }
}
