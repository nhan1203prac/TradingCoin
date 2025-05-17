using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace Coin_Exchange.Models.Enum
{
    public enum WalletTransactionType
    {
        WITHDRAWAL,
	    WALLET_TRANSFER,
	    ADD_MONEY,
	    BUY_ASSET,
	    SELL_ASSET
    }
}
