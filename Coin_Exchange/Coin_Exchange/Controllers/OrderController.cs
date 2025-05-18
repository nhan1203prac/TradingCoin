using Coin_Exchange.Configuration;
using Coin_Exchange.Models.Modal;
using Coin_Exchange.Models;
using Microsoft.AspNetCore.Mvc;
using Coin_Exchange.Service;
using Microsoft.EntityFrameworkCore;
using Coin_Exchange.Models.Request;

namespace Coin_Exchange.Controllers
{
    [Route("api/orders/")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly OrderService _orderService;
        public OrderController(ApplicationDbContext context, OrderService _orderService)
        {
            _context = context;
            _orderService = _orderService;


        }

        [HttpPost("pay")]
        public async Task<ActionResult<Order>> payOrderPayment([FromHeader(Name = "Authorization")] string jwt, [FromBody] createOrderRequest req)
        {

            string enail = JwtProviders.GetEmailFromToken(jwt);
            User user = await _context.Users.FirstOrDefaultAsync(u => u.email == enail);
            Coin coin = await _context.Coins.FirstOrDefaultAsync(c => c.id.Equals(req.coinId));
            Order order = _orderService.processOrder(coin, req.quantity, req.orderType, user);

            return Ok(order);
        }

        [HttpGet("{orderId}")]
        public async Task<ActionResult<Order>> getOrderById([FromHeader(Name = "Authorization")] string jwt, long orderId)
        {

            string enail = JwtProviders.GetEmailFromToken(jwt);
            User user = await _context.Users.FirstOrDefaultAsync(u => u.email == enail);
            Order order = _orderService.getOrderById(orderId);
            if (order.user.id == user.id)
            {
                return Ok(order);
            }
            else
            {
                throw new Exception("you don't have access");
            }


        }

        [HttpGet]
        public async Task<ActionResult<List<Order>>> getAllOrderByUserId([FromHeader(Name = "Authorization")] string jwt)
        {

            string enail = JwtProviders.GetEmailFromToken(jwt);
            User user = await _context.Users.FirstOrDefaultAsync(u => u.email == enail);
            List<Order> order = _orderService.getOrdersOfUser(user.id);
            return Ok(order);


        }
    }
}
