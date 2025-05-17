using Coin_Exchange.Models.Enum;
using Coin_Exchange.Models.Modal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;



namespace Coin_Exchange.Models
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
       : base(options)
        { }

        public DbSet<User> Users { get; set; }
        public DbSet<Coin> Coins { get; set; }
        public DbSet<Asset> Assets { get; set; }
        public DbSet<ForgotPasswordToken> ForgotPasswordTokens { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<PaymentDetails> PaymentDetails { get; set; }
        public DbSet<PaymentOrder> PaymentOrders { get; set; }
        public DbSet<TwoFactorOtp> TwoFactorOtps { get; set; }
        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<Watchlist> Watchlists { get; set; }
        public DbSet<Withdrawal> Withdrawals { get; set; }
        public DbSet<WalletTransaction> WalletTransactions { get; set; }
        public DbSet<WatchlistCoin> WatchlistCoins { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<WatchlistCoin>().ToTable("WatchlistCoin");

            modelBuilder.Entity<Order>()
            .Property(o => o.orderType)
            .HasConversion(
                new ValueConverter<OrderType, string>(
                    v => v.ToString(), // Chuyển enum thành string khi lưu
                    v => (OrderType)System.Enum.Parse(typeof(OrderType), v) // Chuyển string thành enum khi đọc
                ));


            // Chuyển đổi OrderStatus enum thành string khi lưu vào DB
            modelBuilder.Entity<Order>()
                .Property(o => o.orderStatus)
                .HasConversion(
                    new ValueConverter<OrderStatus, string>(
                        v => v.ToString(), // Chuyển enum thành string khi lưu
                        v => (OrderStatus)System.Enum.Parse(typeof(OrderStatus), v) // Chuyển string thành enum khi đọc
                    ));
            modelBuilder.Entity<PaymentOrder>()
         .Property(p => p.status)
         .HasConversion(
             new ValueConverter<PaymentOrderStatus, string>(
                 v => v.ToString(), // Chuyển enum thành string khi lưu vào DB
                 v => (PaymentOrderStatus)System.Enum.Parse(typeof(PaymentOrderStatus), v) // Chuyển string thành enum khi đọc từ DB
             )
         );

            // Cấu hình PaymentMethod enum thành string
            modelBuilder.Entity<PaymentOrder>()
                .Property(p => p.paymentMethod)
                .HasConversion(
                    new ValueConverter<PaymentMethod, string>(
                        v => v.ToString(), // Chuyển enum thành string khi lưu vào DB
                        v => (PaymentMethod)System.Enum.Parse(typeof(PaymentMethod), v) // Chuyển string thành enum khi đọc từ DB
                    )
                );
            modelBuilder.Entity<User>()
        .Property(u => u.role)
        .HasConversion(
            new ValueConverter<USER_ROLE, string>(
                v => v.ToString(), // Chuyển enum thành string khi lưu vào DB
                v => (USER_ROLE)System.Enum.Parse(typeof(USER_ROLE), v) // Chuyển string thành enum khi đọc từ DB
            )
        );
            modelBuilder.Entity<WalletTransaction>()
        .Property(wt => wt.walletTransactionType)
        .HasConversion(
            new ValueConverter<WalletTransactionType, string>(
                v => v.ToString(), // Chuyển enum thành string khi lưu vào DB
                v => (WalletTransactionType)System.Enum.Parse(typeof(WalletTransactionType), v) // Chuyển string thành enum khi đọc từ DB
            )
        );

        }

    }
}
