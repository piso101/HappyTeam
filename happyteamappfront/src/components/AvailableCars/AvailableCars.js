import React, { useState, useEffect } from 'react';
import './AvailableCars.css';
import DetailsPopup from '../DetailsPopUp/DetailsPopUp';

const AvailableCars = ({ unitName, endUnitName, startDate, endDate }) => {
    const [cars, setCars] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const [selectedCar, setSelectedCar] = useState(null);

    useEffect(() => {
        if (unitName && startDate && endDate) {
            console.log("Fetching cars for:", unitName, startDate, endDate);
            fetch(`http://localhost:5146/api/Happy/GetAvailableCars?UnitName=${unitName}&dateBegin=${startDate.toISOString()}&dateEnd=${endDate.toISOString()}`)
                .then(response => {
                    if (!response.ok) {
                        throw new Error('Network response was not ok ' + response.statusText);
                    }
                    return response.json();
                })
                .then(data => {
                    console.log("Fetched cars:", data);
                    setCars(data);
                    setLoading(false);
                })
                .catch(error => {
                    setError(error);
                    setLoading(false);
                });
        }
    }, [unitName, startDate, endDate]);

    const handleBookNow = (car) => {
        setSelectedCar(car);
    };

    const handleClosePopup = () => {
        setSelectedCar(null);
    };

    if (loading) {
        return <div className="loading">Loading...</div>;
    }

    if (error) {
        return <div className="error">Error: {error.message}</div>;
    }

    return (
        <div className="cars-container">
            {cars.map(car => (
                <div key={car.id} className="car-card">
                    <img src={require(`../../assets/CarsImages/${car.image}`)} alt={car.name} className="car-image" />
                    <h3>{car.name}</h3>
                    <p>{car.description}</p>
                    <p>Price per day: ${car.pricePerDay}</p>
                    <button onClick={() => handleBookNow(car)}>Book Now</button>
                </div>
            ))}
            {selectedCar && (
                <DetailsPopup
                    car={selectedCar}
                    startUnit={unitName}
                    endUnit={endUnitName}
                    beginDate={startDate}
                    endDate={endDate}
                    onClose={handleClosePopup}
                />
            )}
        </div>
    );
};

export default AvailableCars;
