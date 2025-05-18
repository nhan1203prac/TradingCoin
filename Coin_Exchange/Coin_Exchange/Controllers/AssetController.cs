using Coin_Exchange.Configuration;
using Coin_Exchange.Models.Modal;
using Coin_Exchange.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Coin_Exchange.Controllers
{
    [Route("api/assets/")]
    [ApiController]
    public class AssetController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AssetController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{assetId}")]
        public async Task<ActionResult<Asset>> getAssetById(long assetId)
        {
            Asset asset = await _context.Assets.FirstOrDefaultAsync(a => a.id == assetId);
            return Ok(asset);

        }

        [HttpGet("coin/{coinId}/user")]
        public async Task<ActionResult<Asset>> getAssetByUserIdAndCoinId(long coinId, [FromHeader(Name = "Authorization")] string jwt)
        {
            string enail = JwtProviders.GetEmailFromToken(jwt);
            User user = await _context.Users.FirstOrDefaultAsync(u => u.email == enail);
            Asset asset = await _context.Assets
             .Include(a => a.coin)
             .Include(a => a.user)
             .FirstOrDefaultAsync(a => a.coin.id.Equals(coinId) && a.user.id == user.id);


            return Ok(asset);
        }


        [HttpGet]
        public async Task<ActionResult<List<Asset>>> getAllAssetOfUser([FromHeader(Name = "Authorization")] string jwt)
        {
            string enail = JwtProviders.GetEmailFromToken(jwt);
            User user = await _context.Users.FirstOrDefaultAsync(u => u.email == enail);
            List<Asset> assets = await _context.Assets
             .Where(a => a.user.id == user.id)
             .ToListAsync();


            return Ok(assets);
        }
    }
}
