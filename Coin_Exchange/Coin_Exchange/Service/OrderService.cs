using Coin_Exchange.Models.Enum;
using Coin_Exchange.Models.Modal;
using Coin_Exchange.Models;
using Microsoft.EntityFrameworkCore;

namespace Coin_Exchange.Service
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;

        public OrderService(ApplicationDbContext context)
        {
            _context = context;
        }

        public Order createOrder(User user, OrderType orderType)
        {
            Order order = new Order();
            order.user = user;
            order.timestamps = DateTime.Now;
            order.orderType = orderType;
            order.orderStatus = OrderStatus.PENDING;
            _context.Orders.Add(order);
            _context.SaveChanges();

            return order;
        }

        public OrderItem createOrderItem(Coin coin, double quantity, double buyPrice, double sellPrice)
        {

            OrderItem orderItem = new OrderItem();
            orderItem.quantity = quantity;
            orderItem.buyPrice = buyPrice;
            orderItem.sellPrice = sellPrice;
            orderItem.coin = coin;

            _context.OrderItems.Add(orderItem);
            _context.SaveChanges();
            return orderItem;
        }

        public Order getOrderById(long orderId)
        {
            Order order = _context.Orders.FirstOrDefault(o => o.id == orderId);
            return order;
        }

        public List<Order> getOrdersOfUser(long userId)
        {
            List<Order> orders = _context.Orders.Where(o => o.user.id == userId).ToList();
            return orders;


        }
        public Wallet payOrderPayment(Order order, User user)
        {
            Wallet wallet = _context.Wallets.FirstOrDefault(w => w.user.id == user.id);
            if (order.orderType.Equals(OrderType.BUY))
            {
                if (wallet.balance < order.price)
                {
                    throw new Exception("Insufficident funds for this transaction");
                }
                Decimal newBalance = wallet.balance - order.price;
                wallet.balance = newBalance;
            }
            else
            {
                Decimal newBalance = wallet.balance + order.price;
                wallet.balance = newBalance;
            }
            _context.SaveChanges();
            return wallet;
        }
        public Asset updateAssetUser(long assetId, double quantity)
        {
            Asset oldAsset = _context.Assets.FirstOrDefault(a => a.id == assetId);
            oldAsset.quantity = quantity + oldAsset.quantity;
            _context.SaveChanges();
            return oldAsset;
        }

        public Order buyAsset(Coin coin, double quantity, User user)
        {

            if (quantity <= 0)
            {
                throw new Exception("quantity should be >0");
            }
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                Order order = createOrder(user, OrderType.BUY);

                OrderItem orderItem = createOrderItem(coin, quantity, coin.currentPrice, 0);
                decimal price = (decimal)orderItem.quantity * (decimal)orderItem.coin.currentPrice;
                order.price = price;
                orderItem.order = order;
                _context.SaveChanges();

                payOrderPayment(order, user);
                order.orderStatus = OrderStatus.SUCCESS;
                _context.SaveChanges();

                Asset oldAsset = _context.Assets
                 .Include(a => a.coin)
                 .Include(a => a.user)
                 .FirstOrDefault(a => a.coin.id.Equals(orderItem.coin.id) && a.user.id == user.id);

                if (oldAsset == null)
                {
                    Asset asset = new Asset();
                    asset.coin = coin;
                    asset.quantity = quantity;
                    asset.user = user;
                    asset.buyPrice = coin.currentPrice;
                    _context.Assets.Add(asset);
                    _context.SaveChanges();
                }
                else
                {
                    oldAsset.quantity = quantity + oldAsset.quantity;
                    _context.SaveChanges();

                }


                return order;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }


        //public Order sellAsset(Coin coin, double quantity, User user)
        //{

        //    if (quantity <= 0)
        //    {
        //        throw new Exception("quantity should be >0");
        //    }

        //    Asset oldAsset = _context.Assets
        //     .Include(a => a.coin)
        //     .Include(a => a.user)
        //     .FirstOrDefault(a => a.coin.id.Equals(coin.id) && a.user.id == user.id);
        //    if (oldAsset != null)
        //    {
        //        double buyPrice = oldAsset.buyPrice;
        //        Order order = createOrder(user, OrderType.SELL);
        //        OrderItem orderItem = createOrderItem(coin, quantity,buyPrice, coin.currentPrice);
        //        decimal price = (decimal)orderItem.quantity * (decimal)orderItem.coin.currentPrice;
        //        order.price = price;
        //        orderItem.order = order;
        //        _context.SaveChanges();


        //        if (oldAsset.quantity >= quantity)
        //        {
        //            order.orderStatus = OrderStatus.SUCCESS;
        //            _context.SaveChanges();
        //            payOrderPayment(order, user);
        //            Asset updateAsset = updateAssetUser(oldAsset.id, -quantity);
        //            if(updateAsset.quantity == 0)
        //            {
        //                _context.Assets.Remove(oldAsset);
        //            }
        //            return order;
        //        }
        //        throw new Exception("Insufficident quantity to sell");
        //    }
        //    throw new Exception("Asset not found");







        //}
        public Order sellAsset(Coin coin, double quantity, User user)
        {
            if (quantity <= 0)
            {
                throw new ArgumentException("Quantity should be > 0", nameof(quantity));
            }

            using var transaction = _context.Database.BeginTransaction();
            try
            {

                Asset oldAsset = _context.Assets
                    .Include(a => a.coin)
                    .Include(a => a.user)
                    .FirstOrDefault(a => a.coin.id == coin.id && a.user.id == user.id);

                if (oldAsset == null)
                {
                    throw new InvalidOperationException("Asset not found");
                }

                if (oldAsset.quantity < quantity)
                {
                    throw new InvalidOperationException("Insufficient quantity to sell");
                }


                double buyPrice = oldAsset.buyPrice;
                Order order = createOrder(user, OrderType.SELL);
                OrderItem orderItem = createOrderItem(coin, quantity, buyPrice, coin.currentPrice);


                decimal price = (decimal)orderItem.quantity * (decimal)orderItem.coin.currentPrice;
                if (price <= 0)
                {
                    throw new InvalidOperationException("Price must be greater than zero.");
                }

                order.price = price;
                orderItem.order = order;


                _context.SaveChanges();


                payOrderPayment(order, user);


                oldAsset.quantity -= quantity;
                if (oldAsset.quantity == 0)
                {
                    _context.Assets.Remove(oldAsset);
                }


                order.orderStatus = OrderStatus.SUCCESS;
                _context.SaveChanges();


                transaction.Commit();

                return order;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }


        public Order processOrder(Coin coin, double quantity, OrderType orderType, User user)
        {
            if (orderType.Equals(OrderType.BUY))
            {
                return buyAsset(coin, quantity, user);
            }
            else
            {
                return sellAsset(coin, quantity, user);
            }
            throw new Exception("Invalid order type");
        }
    }


    public interface IOrderService
    {
        Order createOrder(User user, OrderType orderType);

        OrderItem createOrderItem(Coin coin, double quantity, double buyPrice, double sellPrice);
        Order getOrderById(long orderId);

    }
}
