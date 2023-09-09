import React, { Component } from 'react';
import {Input} from "reactstrap";
import {MedianChart} from "./MedianChart";
import {formatPrice} from "../utilites/formater";
import {getLastPrice} from "../utilites/dataHelper";

export class Apartments extends Component {

  constructor(props) {
    super(props);
    this.state = {
      apartments: [],
      filteredApartments: [],
      selectedRoomCount: '',
      loading: true
    };
  }

  async componentDidMount() {
    await this.populateData();
  }

  handleRoomFilterChange = (e) => {
    const roomCount = e.target.value;
    this.setState({ selectedRoomCount: roomCount }, () => {
      this.filterApartments();
    });
  };

  filterApartments = () => {
    const { apartments, selectedRoomCount } = this.state;

    if (selectedRoomCount === '') {
      this.setState({ filteredApartments: apartments });
    } else {
      const filtered = apartments.filter(
          (apartment) => apartment.roomsCount === parseInt(selectedRoomCount)
      );
      this.setState({ filteredApartments: filtered });
    }
  };

  static renderTable(apartments, roomsCount) {
    return (
        <>
          <table className="table table-striped" aria-labelledby="tableLabel">
            <thead>
            <tr>
              <th>Комнаты</th>
              <th>Стоимость</th>
              <th>Сайт застройщика</th>
            </tr>
            </thead>
            <tbody>
            {apartments.map(apartment => {
                  let lastPrice = getLastPrice(apartment.priceHistory);
                  return <tr key={apartment.id}>
                    <td>{apartment.roomsCount}</td>
                    <td>{formatPrice(lastPrice)}</td>
                    <td><a href={apartment.developerSiteLink} className="text-dark">Перейти</a></td>
                  </tr>;
                }
            )}
            </tbody>
          </table>
          <div>
            <h3>График медианы стоимости квартир</h3>
            <MedianChart priceHistoriesByApertments={apartments.map(d => d.priceHistory)} roomsCount={roomsCount}/>
          </div>
        </>
    );
  }
  
  render() {
    const { filteredApartments, selectedRoomCount } = this.state;

    let contents = this.state.loading
      ? <p><em>Loading...</em></p>
      : filteredApartments.length === 0 
            ? <p><br/><em>Квартиры по заданному фильтру не найдены</em></p>
            : Apartments.renderTable(filteredApartments, selectedRoomCount);
    
    return (
      <div>
        <h1>Квартиры в наличии</h1>
        <div>
          <Input
              bsSize="sm"
              type="number"
              placeholder="Фильтр по количеству комнат"
              value={selectedRoomCount}
              onChange={this.handleRoomFilterChange}
          />
        </div>
        {contents}
      </div>
    );
  }

  async populateData() {
    const response = await fetch('apartments');
    const data = await response.json();
    this.setState({ apartments: data, filteredApartments: data, loading: false });
  }
}
