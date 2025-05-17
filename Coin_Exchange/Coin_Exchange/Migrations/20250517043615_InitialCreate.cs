using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Coin_Exchange.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Coins",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    symbol = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    image = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    currentPrice = table.Column<double>(type: "float", nullable: false),
                    marketCap = table.Column<long>(type: "bigint", nullable: false),
                    marketCapRank = table.Column<int>(type: "int", nullable: false),
                    fullyDilutedValuation = table.Column<double>(type: "float", nullable: false),
                    totalVolume = table.Column<long>(type: "bigint", nullable: false),
                    high24h = table.Column<double>(type: "float", nullable: false),
                    low24h = table.Column<double>(type: "float", nullable: false),
                    priceChange24h = table.Column<double>(type: "float", nullable: false),
                    priceChangePercentage24h = table.Column<double>(type: "float", nullable: false),
                    marketCapChange24h = table.Column<long>(type: "bigint", nullable: false),
                    marketCapChangePercentage24h = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coins", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsEnable2FA = table.Column<bool>(type: "bit", nullable: false),
                    role = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Assets",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    quantity = table.Column<double>(type: "float", nullable: false),
                    buyPrice = table.Column<double>(type: "float", nullable: false),
                    CoinId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    UserId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assets", x => x.id);
                    table.ForeignKey(
                        name: "FK_Assets_Coins_CoinId",
                        column: x => x.CoinId,
                        principalTable: "Coins",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Assets_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ForgotPasswordTokens",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    otp = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    sendTo = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ForgotPasswordTokens", x => x.id);
                    table.ForeignKey(
                        name: "FK_ForgotPasswordTokens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    orderType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    timestamps = table.Column<DateTime>(type: "datetime2", nullable: false),
                    orderStatus = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.id);
                    table.ForeignKey(
                        name: "FK_Orders_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PaymentDetails",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    accountNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    accountHolderName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ifsc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    bankName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentDetails", x => x.id);
                    table.ForeignKey(
                        name: "FK_PaymentDetails_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PaymentOrders",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    amount = table.Column<long>(type: "bigint", nullable: false),
                    status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    paymentMethod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    userId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentOrders", x => x.id);
                    table.ForeignKey(
                        name: "FK_PaymentOrders_Users_userId",
                        column: x => x.userId,
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TwoFactorOtps",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    otp = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    User = table.Column<long>(type: "bigint", nullable: false),
                    jwt = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TwoFactorOtps", x => x.id);
                    table.ForeignKey(
                        name: "FK_TwoFactorOtps_Users_User",
                        column: x => x.User,
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Wallets",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    balance = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wallets", x => x.id);
                    table.ForeignKey(
                        name: "FK_Wallets_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Watchlists",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    userId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Watchlists", x => x.id);
                    table.ForeignKey(
                        name: "FK_Watchlists_Users_userId",
                        column: x => x.userId,
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Withdrawals",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    status = table.Column<int>(type: "int", nullable: false),
                    userId = table.Column<long>(type: "bigint", nullable: false),
                    amount = table.Column<long>(type: "bigint", nullable: false),
                    date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Withdrawals", x => x.id);
                    table.ForeignKey(
                        name: "FK_Withdrawals_Users_userId",
                        column: x => x.userId,
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    quantity = table.Column<double>(type: "float", nullable: false),
                    CoinId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    buyPrice = table.Column<double>(type: "float", nullable: false),
                    sellPrice = table.Column<double>(type: "float", nullable: false),
                    OrderId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => x.id);
                    table.ForeignKey(
                        name: "FK_OrderItems_Coins_CoinId",
                        column: x => x.CoinId,
                        principalTable: "Coins",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_OrderItems_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WalletTransactions",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WalletId = table.Column<long>(type: "bigint", nullable: false),
                    walletTransactionType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    datel = table.Column<DateTime>(type: "datetime2", nullable: false),
                    transferId = table.Column<long>(type: "bigint", nullable: false),
                    purpose = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WalletTransactions", x => x.id);
                    table.ForeignKey(
                        name: "FK_WalletTransactions_Wallets_WalletId",
                        column: x => x.WalletId,
                        principalTable: "Wallets",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WatchlistCoin",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WatchlistId = table.Column<long>(type: "bigint", nullable: false),
                    CoinId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WatchlistCoin", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WatchlistCoin_Coins_CoinId",
                        column: x => x.CoinId,
                        principalTable: "Coins",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WatchlistCoin_Watchlists_WatchlistId",
                        column: x => x.WatchlistId,
                        principalTable: "Watchlists",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Assets_CoinId",
                table: "Assets",
                column: "CoinId");

            migrationBuilder.CreateIndex(
                name: "IX_Assets_UserId",
                table: "Assets",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ForgotPasswordTokens_UserId",
                table: "ForgotPasswordTokens",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_CoinId",
                table: "OrderItems",
                column: "CoinId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderId",
                table: "OrderItems",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UserId",
                table: "Orders",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentDetails_UserId",
                table: "PaymentDetails",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentOrders_userId",
                table: "PaymentOrders",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_TwoFactorOtps_User",
                table: "TwoFactorOtps",
                column: "User");

            migrationBuilder.CreateIndex(
                name: "IX_Wallets_UserId",
                table: "Wallets",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_WalletTransactions_WalletId",
                table: "WalletTransactions",
                column: "WalletId");

            migrationBuilder.CreateIndex(
                name: "IX_WatchlistCoin_CoinId",
                table: "WatchlistCoin",
                column: "CoinId");

            migrationBuilder.CreateIndex(
                name: "IX_WatchlistCoin_WatchlistId",
                table: "WatchlistCoin",
                column: "WatchlistId");

            migrationBuilder.CreateIndex(
                name: "IX_Watchlists_userId",
                table: "Watchlists",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_Withdrawals_userId",
                table: "Withdrawals",
                column: "userId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Assets");

            migrationBuilder.DropTable(
                name: "ForgotPasswordTokens");

            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "PaymentDetails");

            migrationBuilder.DropTable(
                name: "PaymentOrders");

            migrationBuilder.DropTable(
                name: "TwoFactorOtps");

            migrationBuilder.DropTable(
                name: "WalletTransactions");

            migrationBuilder.DropTable(
                name: "WatchlistCoin");

            migrationBuilder.DropTable(
                name: "Withdrawals");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Wallets");

            migrationBuilder.DropTable(
                name: "Coins");

            migrationBuilder.DropTable(
                name: "Watchlists");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
