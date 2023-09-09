import {useState, useEffect} from 'react';
import {ChartLine} from "./ChartLine";
import {addQuantitativeRoomsEnding, formatDateToMonthYear} from "../utilites/formater";
import {calculateMedian} from "../utilites/medianHelper";

function buildDataSet(priceHistory) {
    const pricesByDate = groupPricesByDate(priceHistory);
    const medianApartmentPricesByDate = {
        labels: [],
        data: []
    };

    for (let priceByDate of pricesByDate) {
        medianApartmentPricesByDate.labels.push(formatDateToMonthYear(priceByDate[0]))
        medianApartmentPricesByDate.data.push(calculateMedian(priceByDate[1]))
    }
    return medianApartmentPricesByDate;
}

function groupPricesByDate(priceHistory) {
    let pricesByDate = new Map();

    priceHistory.forEach(subArray => {
        subArray.forEach(item => {
            let date = item.priceChangeDate;
            let price = item.price;
            if (pricesByDate.has(date)) {
                pricesByDate.get(date).push(price);
            } else {
                pricesByDate.set(date, [price]);
            }
        });
    });

    return pricesByDate;
}

export const MedianChart = (props) => {
    const [data, setData] = useState({});
    const [textLabel, setTextLabel] = useState('');

    useEffect(() => {
        const medianApartmentPricesByDate = buildDataSet(props.priceHistoriesByApertments);
        setData(medianApartmentPricesByDate);
        
        setTextLabel(`Медианная стоимость ${addQuantitativeRoomsEnding(props.roomsCount)} квартир`)
    }, [props.priceHistoriesByApertments, props.roomsCount]);
    
    return (
        <ChartLine data={data} textLabel={textLabel}/>
    )
};