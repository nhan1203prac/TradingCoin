export const ExitCoinInWatchlist = (items, coin) => {
    return items.some(element => element.id == coin?.id);
};
