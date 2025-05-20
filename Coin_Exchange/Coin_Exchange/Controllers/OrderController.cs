using Coin_Exchange.Configuration;
using Coin_Exchange.Models.Modal;
using Coin_Exchange.Models;
using Microsoft.AspNetCore.Mvc;
using Coin_Exchange.Service;
using Microsoft.EntityFrameworkCore;
using Coin_Exchange.Models.Request;
using Coin_Exchange.Models.Response;

namespace Coin_Exchange.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IOrderService _orderService;
        public OrderController(ApplicationDbContext context, IOrderService orderService)
        {
            _context = context;
            _orderService = orderService;


        }

        [HttpPost("pay")]
        public async Task<ActionResult<Order>> payOrderPayment([FromHeader(Name = "Authorization")] string jwt, [FromBody] createOrderRequest req)
        {

            if (string.IsNullOrEmpty(jwt))
            {
                return BadRequest("Authorization token is missing.");
            }

            string email = JwtProviders.GetEmailFromToken(jwt);
            if (string.IsNullOrEmpty(email))
            {
                return Unauthorized("Invalid token.");
            }


            User user = await _context.Users.FirstOrDefaultAsync(u => u.email == email);
            if (user == null)
            {
                return Unauthorized("User not found.");
            }


            if (req.coinId == null)
            {
                return BadRequest("Invalid coin ID.");
            }


            Coin coin = await _context.Coins.FirstOrDefaultAsync(c => c.id == req.coinId);
            if (coin == null)
            {
                return BadRequest("Coin not found.");
            }


            if (string.IsNullOrEmpty(req.orderType))
            {
                return BadRequest("Order type is required.");
            }


            if (string.IsNullOrEmpty(req.quantity) || !double.TryParse(req.quantity, out double parsedQuantity))
            {
                return BadRequest("Invalid quantity.");
            }

            Order order = null;

            try
            {
                double quantity = req.GetQuantityAsDouble();
                if (_orderService == null)
                {
                    return BadRequest("Order service is not available.");
                }


                order = await _orderService.processOrder(coin, quantity, req.orderType, user);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error processing order: {ex.Message}");
            }


            if (order != null)
            {
                return Ok(order);
            }

            return BadRequest("Order could not be processed.");
        }

        [HttpGet("{orderId}")]
        public async Task<ActionResult<Order>> getOrderById([FromHeader(Name = "Authorization")] string jwt, long orderId)
        {

            string email = JwtProviders.GetEmailFromToken(jwt);
            User user = await _context.Users.FirstOrDefaultAsync(u => u.email == email);
            if (user == null)
            {
                return Unauthorized("User not found.");
            }

            Order order = await _orderService.getOrderById(orderId);
            if (order == null)
            {
                return NotFound("Order not found.");
            }

            if (order.user.id == user.id)
            {
                return Ok(order);
            }
            else
            {
                return Unauthorized("You don't have access to this order.");
            }


        }

        [HttpGet]
        public async Task<ActionResult<List<Order>>> getAllOrderByUserId([FromHeader(Name = "Authorization")] string jwt)
        {
            string email = JwtProviders.GetEmailFromToken(jwt);
            User user = await _context.Users.FirstOrDefaultAsync((u => u.email == email));
            if (user == null)
            {
                return Unauthorized("User not found.");
            }

            List<OrderItemDTO> orders = await _orderService.getOrdersOfUser(user.id);
            return Ok(orders);


        }
    }
}
