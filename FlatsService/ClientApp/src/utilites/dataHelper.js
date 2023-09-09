export function getLastPrice(priceHistory) {
    if (priceHistory.length === 0) {
        return null;
    }

    return priceHistory.reduce((max, current) => {
        const maxDate = new Date(max.priceChangeDate);
        const currentDate = new Date(current.priceChangeDate);

        return maxDate > currentDate ? max : current;
    }, priceHistory[0]).price;
}
