import {useState, useRef, useEffect} from 'react';
import {Line} from 'react-chartjs-2';
import 'chart.js/auto';
import {formatPrice} from "../utilites/formater";

const defaultData = {
    labels: [],
    datasets: [
        {
            label: '',
            data: [],
            fill: true,
            backgroundColor: 'rgba(75,192,192,0.2)',
            borderColor: 'rgba(75,192,192,1)'
        }
    ],
}

export const ChartLine = (props) => {
    const [dataSet, setDataSet] = useState(defaultData)
    const ref = useRef();

    useEffect(() => {
        setDataSet({
            labels: props.data.labels,
            datasets: [
                {
                    label: props.textLabel,
                    data: props.data.data,
                    fill: true,
                    backgroundColor: 'rgba(75,192,192,0.2)',
                    borderColor: 'rgba(75,192,192,1)'
                }
            ],
        })
    }, [props.data, props.textLabel]);

    const options = {
        scales: {
            y: {
                ticks: {
                    callback: function (value, index, ticks) {
                        return formatPrice(value);
                    }
                }
            }
        }
    }

    return <Line ref={ref} data={dataSet} options={options}/>
};