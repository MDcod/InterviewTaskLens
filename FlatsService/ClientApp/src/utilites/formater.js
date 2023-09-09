export function formatPrice(price) {
    return new Intl.NumberFormat('ru-RU', {
        style: 'currency',
        currency: 'RUB',
        minimumFractionDigits: 0, // Убираем дробные части
    }).format(parseInt(price));
}

export function formatDateToMonthYear(inputDate) {
    const date = new Date(inputDate);
    const monthNames = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
    const month = monthNames[date.getMonth()];
    const year = date.getFullYear();
    return `${month} ${year}`;
}

export function addQuantitativeRoomsEnding(number) {
    if (number === '') {
        return 'всех'
    }
    if (parseInt(number) > 10 && parseInt(number) < 15) {
        return `${number}и комнатных`
    }
    if (['5', '6', '7', '8', '9', '0'].includes(number.slice(-1))) {
        return `${number}и комнатных`
    }
    if (['2', '3', '4'].includes(number.slice(-1))) {
        return `${number}х`
    }
    if (number.slice(-1) === '1') {
        return `${number}но комнатных`
    }
}