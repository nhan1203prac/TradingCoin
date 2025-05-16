using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Trading_coin.Models.Modal;

namespace Trading_coin.Models.Enum
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
