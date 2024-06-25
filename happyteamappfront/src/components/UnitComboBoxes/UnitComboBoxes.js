import React, { useEffect, useState } from 'react';
import DateChooser from '../DateChooser/DateChooser.js';
import AvailableCars from '../AvailableCars/AvailableCars';
import './UnitComboBoxes.css';
import config from '../../config';

const UnitsComponent = () => {
    const [units, setUnits] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const [startLocation, setStartLocation] = useState('');
    const [endLocation, setEndLocation] = useState('');
    const [startDate, setStartDate] = useState(null);
    const [endDate, setEndDate] = useState(null);

    useEffect(() => {
        fetch(`${config.apiBaseUrl}/api/Happy/Units`)
            .then(response => {
                if (!response.ok) {
                    throw new Error('Network response was not ok ' + response.statusText);
                }
                return response.json();
            })
            .then(data => {
                setUnits(data);
                setLoading(false);
            })
            .catch(error => {
                setError(error);
                setLoading(false);
            });
    }, []);

    if (loading) {
        return <div className="loading">Loading...</div>;
    }

    if (error) {
        return <div className="error">Error: {error.message}</div>;
    }

    const handleStartLocationChange = (event) => {
        setStartLocation(event.target.value);
    };

    const handleEndLocationChange = (event) => {
        setEndLocation(event.target.value);
    };

    const handleDateChange = (start, end) => {
        setStartDate(start);
        setEndDate(end);
    };

    return (
        <div className="container">
            <h1>Units</h1>
            <div className="selector-container">
                <div className="divComboClass">
                    <label htmlFor="start-location">Start Location: </label>
                    <select id="start-location" value={startLocation} onChange={handleStartLocationChange}>
                        <option value="">Select a location</option>
                        {units.map(unit => (
                            <option key={unit.id} value={unit.locationName}>
                                {unit.locationName}
                            </option>
                        ))}
                    </select>
                </div>
                <div className="divComboClass">
                    <label htmlFor="end-location">End Location: </label>
                    <select id="end-location" value={endLocation} onChange={handleEndLocationChange}>
                        <option value="">Select a location</option>
                        {units.map(unit => (
                            <option key={unit.id} value={unit.locationName}>
                                {unit.locationName}
                            </option>
                        ))}
                    </select>
                </div>
                <DateChooser onDateChange={handleDateChange} />
            </div>
            {startLocation && endLocation && startDate && endDate && (
                <AvailableCars
                    unitName={startLocation}
                    endUnitName={endLocation}
                    startDate={startDate}
                    endDate={endDate}
                />
            )}
        </div>
    );
};

export default UnitsComponent;