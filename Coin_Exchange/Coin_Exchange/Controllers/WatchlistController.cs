using Coin_Exchange.Configuration;
using Coin_Exchange.Models.Modal;
using Coin_Exchange.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Coin_Exchange.Models.Response;

namespace Coin_Exchange.Controllers
{
    [Route("api/watchlist/")]
    [ApiController]
    public class WatchlistController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public WatchlistController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("user")]
        public async Task<ActionResult<List<Coin>>> GetUserWatchlist([FromHeader(Name = "Authorization")] string jwt)
        {
            try
            {
                string email = JwtProviders.GetEmailFromToken(jwt);
                User user = await _context.Users.FirstOrDefaultAsync(u => u.email == email);
                if (user == null)
                {
                    return NotFound("User not found.");
                }

                Watchlist watchlist = await _context.Watchlists
                    .Include(w => w.WatchlistCoins)
                        .ThenInclude(wc => wc.Coin)
                    .FirstOrDefaultAsync(w => w.userId == user.id);


                if (watchlist == null || watchlist.WatchlistCoins == null || !watchlist.WatchlistCoins.Any())
                {
                    return Ok(new List<Coin>());
                }


                List<Coin> userCoins = watchlist.WatchlistCoins
                                                .Where(wc => wc.Coin != null)
                                                .Select(wc => wc.Coin)
                                                .ToList();


                return Ok(userCoins);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }



        [HttpPost("create")]
        public async Task<ActionResult<Watchlist>> createWatchList([FromHeader(Name = "Authorization")] string jwt)
        {
            string enail = JwtProviders.GetEmailFromToken(jwt);
            User user = await _context.Users.FirstOrDefaultAsync(u => u.email == enail);
            Watchlist watchlist = new Watchlist
            {
                user = user
            };
            _context.Watchlists.Add(watchlist);
            await _context.SaveChangesAsync();

            return Ok(watchlist);
        }


        [HttpGet("{watchlistId}")]
        public async Task<ActionResult<Watchlist>> getWatchListById(long watchlistId)
        {
            Watchlist watchlist = await _context.Watchlists.FirstOrDefaultAsync(c => c.id == watchlistId);
            return Ok(watchlist);
        }





        [HttpPatch("add/coin/{coinId}")]
        public async Task<ActionResult<Coin>> AddItemToWatchList([FromHeader(Name = "Authorization")] string jwt, string coinId)
        {
            try
            {
                string email = JwtProviders.GetEmailFromToken(jwt);

                User user = await _context.Users.FirstOrDefaultAsync(u => u.email == email);
                if (user == null)
                {
                    return NotFound("User not found.");
                }

                Coin coin = await _context.Coins.FirstOrDefaultAsync(c => c.id == coinId);
                if (coin == null)
                {
                    return NotFound("Coin not found.");
                }

                Watchlist watchlist = await _context.Watchlists
                    .Include(w => w.WatchlistCoins)
                    .FirstOrDefaultAsync(w => w.user.id == user.id);
                if (watchlist == null)
                {
                    return NotFound("Watchlist not found.");
                }

                List<WatchlistCoin> existingWatchlistCoin = watchlist.WatchlistCoins.Where(wc => wc.WatchlistId == watchlist.id).ToList();

                if (existingWatchlistCoin.Any())
                {
                    bool isExisting = false;
                    foreach (WatchlistCoin wl in existingWatchlistCoin)
                    {
                        if (wl.CoinId == coinId)
                        {
                            isExisting = true;
                            _context.WatchlistCoins.Remove(wl);
                            await _context.SaveChangesAsync();
                            break;
                        }
                    }

                    if (!isExisting)
                    {


                        var newWatchlistCoin = new WatchlistCoin
                        {

                            WatchlistId = watchlist.id,
                            CoinId = coin.id
                        };

                        _context.WatchlistCoins.Add(newWatchlistCoin);
                        await _context.SaveChangesAsync();
                    }
                }
                else
                {


                    var newWatchlistCoin = new WatchlistCoin
                    {

                        WatchlistId = watchlist.id,
                        CoinId = coin.id
                    };

                    _context.WatchlistCoins.Add(newWatchlistCoin);
                    await _context.SaveChangesAsync();
                }

                return Ok(coin);
            }
            catch (DbUpdateException dbEx)
            {
                // Ghi chi tiết lỗi từ DbUpdateException, bao gồm cả InnerException
                return StatusCode(StatusCodes.Status500InternalServerError, $"Database error: {dbEx.Message}, Inner Exception: {dbEx.InnerException?.Message}");
            }
            catch (Exception ex)
            {
                // Ghi chi tiết lỗi từ Exception
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}, Inner Exception: {ex.InnerException?.Message}");
            }
        }


    }
}
