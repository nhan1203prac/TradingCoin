using Coin_Exchange.Models.Enum;
using Coin_Exchange.Models.Modal;
using Coin_Exchange.Models;
using Microsoft.EntityFrameworkCore;
using Coin_Exchange.Models.Response;

namespace Coin_Exchange.Service
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;

        public OrderService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Order> createOrder(User user, OrderType orderType)
        {
            Order order = new Order
            {

                user = user,
                timestamps = DateTime.Now,
                orderType = orderType,
                orderStatus = OrderStatus.PENDING
            };

            try
            {
                _context.Orders.Add(order);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error saving order: " + ex.Message);
            }

            return order;
        }

        public async Task<OrderItem> createOrderItem(Coin coin, double quantity, double buyPrice, double sellPrice, Order order)
        {
            OrderItem orderItem = new OrderItem
            {
                quantity = quantity,
                buyPrice = buyPrice,
                sellPrice = sellPrice,
                coin = coin,
                order = order
            };

            try
            {
                _context.OrderItems.Add(orderItem);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error saving order item: " + ex.Message);
            }

            return orderItem;
        }

        public async Task<Order> getOrderById(long orderId)
        {
            Order order = await _context.Orders.FirstOrDefaultAsync(o => o.id == orderId);
            return order;
        }

        public async Task<List<OrderItemDTO>> getOrdersOfUser(long userId)
        {
            var orderItems = await _context.OrderItems
               .Where(oi => oi.order != null && oi.order.UserId == userId)
               .Include(oi => oi.coin)
               .Include(oi => oi.order)
               .Select(oi => new OrderItemDTO
               {
                   Id = oi.id,
                   Quantity = oi.quantity,
                   BuyPrice = oi.buyPrice,
                   SellPrice = oi.sellPrice,
                   Coin = oi.coin,
                   orderType = oi.order.orderType.ToString(),
                   price = oi.order.price,
            
                   Timestamps = oi.order.timestamps
               })
               .ToListAsync();

            return orderItems;
        }



        public async Task<Wallet> payOrderPayment(Order order, User user)
        {
            Wallet wallet = await _context.Wallets.FirstOrDefaultAsync(w => w.user.id == user.id);
            if (order.orderType == OrderType.BUY)
            {
                if (wallet.balance < order.price)
                {
                    throw new Exception("Insufficient funds for this transaction");
                }
                Decimal newBalance = wallet.balance - order.price;
                wallet.balance = newBalance;
            }
            else
            {
                Decimal newBalance = wallet.balance + order.price;
                wallet.balance = newBalance;
            }
            await _context.SaveChangesAsync();
            return wallet;
        }

        public async Task<Asset> updateAssetUser(long assetId, double quantity)
        {
            Asset oldAsset = await _context.Assets.FirstOrDefaultAsync(a => a.id == assetId);
            oldAsset.quantity = quantity + oldAsset.quantity;
            await _context.SaveChangesAsync();
            return oldAsset;
        }

        public async Task<Order> buyAsset(Coin coin, double quantity, User user)
        {
            if (quantity <= 0)
            {
                throw new Exception("quantity should be >0");
            }

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                Order order = await createOrder(user, OrderType.BUY);
                if (order == null)
                {
                    throw new Exception("order is null");
                }

                OrderItem orderItem = await createOrderItem(coin, quantity, coin.currentPrice, 0, order);
                if (orderItem == null)
                {
                    throw new Exception("orderItem is null");
                }


                decimal price = (decimal)orderItem.quantity * (decimal)orderItem.coin.currentPrice;
                order.price = price;

                await _context.SaveChangesAsync();

                await payOrderPayment(order, user);
                order.orderStatus = OrderStatus.SUCCESS;
                await _context.SaveChangesAsync();

                Asset oldAsset = await _context.Assets
                 .Include(a => a.coin)
                 .Include(a => a.user)
                 .FirstOrDefaultAsync(a => a.coin.id.Equals(orderItem.coin.id) && a.user.id == user.id);

                if (oldAsset == null)
                {
                    Asset asset = new Asset
                    {
                        coin = coin,
                        quantity = quantity,
                        user = user,
                        buyPrice = coin.currentPrice
                    };
                    _context.Assets.Add(asset);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    oldAsset.quantity = quantity + oldAsset.quantity;
                    await _context.SaveChangesAsync();
                }

                await transaction.CommitAsync();
                return order;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<Order> sellAsset(Coin coin, double quantity, User user)
        {
            if (quantity <= 0)
            {
                throw new ArgumentException("Quantity should be > 0", nameof(quantity));
            }

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                Asset oldAsset = await _context.Assets
                    .Include(a => a.coin)
                    .Include(a => a.user)
                    .FirstOrDefaultAsync(a => a.coin.id == coin.id && a.user.id == user.id);

                if (oldAsset == null)
                {
                    throw new InvalidOperationException("Asset not found");
                }

                if (oldAsset.quantity < quantity)
                {
                    throw new InvalidOperationException("Insufficient quantity to sell");
                }

                double buyPrice = oldAsset.buyPrice;
                Order order = await createOrder(user, OrderType.SELL);
                OrderItem orderItem = await createOrderItem(coin, quantity, buyPrice, coin.currentPrice, order);

                decimal price = (decimal)orderItem.quantity * (decimal)orderItem.coin.currentPrice;
                if (price <= 0)
                {
                    throw new InvalidOperationException("Price must be greater than zero.");
                }

                order.price = price;
                await _context.SaveChangesAsync();

                await payOrderPayment(order, user);

                oldAsset.quantity -= quantity;
                if (oldAsset.quantity == 0)
                {
                    _context.Assets.Remove(oldAsset);
                }

                order.orderStatus = OrderStatus.SUCCESS;
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
                return order;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<Order> processOrder(Coin coin, double quantity, string orderType, User user)
        {
            Console.WriteLine("Coin in processorder");

            if (coin == null) throw new ArgumentNullException(nameof(coin), "Coin cannot be null.");
            if (user == null) throw new ArgumentNullException(nameof(user), "User cannot be null.");
            if (string.IsNullOrEmpty(orderType)) throw new ArgumentException("Order type cannot be null or empty.", nameof(orderType));

            if (orderType.Equals(OrderType.BUY.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                return await buyAsset(coin, quantity, user);
            }
            else if (orderType.Equals(OrderType.SELL.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                return await sellAsset(coin, quantity, user);
            }

            throw new Exception("Invalid order type");
        }

    }

    public interface IOrderService
    {
        Task<Order> createOrder(User user, OrderType orderType);
        Task<OrderItem> createOrderItem(Coin coin, double quantity, double buyPrice, double sellPrice, Order order);
        Task<Order> getOrderById(long orderId);
        Task<List<OrderItemDTO>> getOrdersOfUser(long userId);
        Task<Wallet> payOrderPayment(Order order, User user);
        Task<Asset> updateAssetUser(long assetId, double quantity);
        Task<Order> buyAsset(Coin coin, double quantity, User user);
        Task<Order> sellAsset(Coin coin, double quantity, User user);
        Task<Order> processOrder(Coin coin, double quantity, string orderType, User user);
    }
}
