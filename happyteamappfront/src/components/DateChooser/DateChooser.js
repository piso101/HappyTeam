import React, { useState } from 'react';
import DatePicker from 'react-datepicker';
import 'react-datepicker/dist/react-datepicker.css';
import './DateChooser.css';

const DateChooser = ({ onDateChange }) => {
    const [startDate, setStartDate] = useState(null);
    const [endDate, setEndDate] = useState(null);

    const handleStartDateChange = (date) => {
        setStartDate(date);
        onDateChange(date, endDate);
    };

    const handleEndDateChange = (date) => {
        setEndDate(date);
        onDateChange(startDate, date);
    };

    return (
        <div className="date-chooser">
            <div className="date-picker-container">
                <label htmlFor="start-date">Start Date: </label>
                <DatePicker
                    id="start-date"
                    selected={startDate}
                    onChange={handleStartDateChange}
                    selectsStart
                    startDate={startDate}
                    endDate={endDate}
                    dateFormat="MMMM d, yyyy"
                    placeholderText="Select start date"
                />
            </div>
            <div className="date-picker-container">
                <label htmlFor="end-date">End Date: </label>
                <DatePicker
                    id="end-date"
                    selected={endDate}
                    onChange={handleEndDateChange}
                    selectsEnd
                    startDate={startDate}
                    endDate={endDate}
                    minDate={startDate}
                    dateFormat="MMMM d, yyyy"
                    placeholderText="Select end date"
                />
            </div>
        </div>
    );
};

export default DateChooser;
