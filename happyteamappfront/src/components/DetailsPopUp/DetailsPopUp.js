import React, { useState } from 'react';
import './DetailsPopUp.css';
import config from '../../config';

const DetailsPopup = ({ car, startUnit, endUnit, beginDate, endDate, onClose }) => {
    const [firstName, setFirstName] = useState('');
    const [lastName, setLastName] = useState('');
    const [email, setEmail] = useState('');
    const [error, setError] = useState('');
    const [successMessage, setSuccessMessage] = useState('');

    const handleSubmit = async () => {
        if (!firstName || !lastName || !email) {
            setError('All fields are required.');
            return;
        }

        const requestBody = {
            User: {
                Id: "Default",
                Name: firstName,
                Surname: lastName,
                Email: email,
                Token: '',
                DateOfCreationOfToken: new Date().toISOString()  // ensure date is in ISO format
            },
            Car: {
                Id: car.id,
                Name: car.name,
                Description: car.description,
                Image: car.image,
                PricePerDay: car.pricePerDay,
                UnitId: car.unitId
            },
            Order: {
                Id: "Default",
                StartDate: beginDate.toISOString(),
                EndDate: endDate.toISOString(),
                TotalPrice: car.pricePerDay * ((endDate - beginDate) / (1000 * 60 * 60 * 24)),
                CarId: car.id,
                UserId: '', // To be set by the server
                StartUnit: startUnit,
                EndUnit: endUnit,
                IsVerified: false
            }
        };

        console.log('Request Body:', JSON.stringify(requestBody, null, 2));

        try {
            const response = await fetch(`${config.apiBaseUrl}/api/Happy/EmailVer`, {
                method: 'POST',
                headers: {
                    'Content-Type': "application/json",
                },
                body: JSON.stringify(requestBody)
            });

            if (!response.ok) {
                setSuccessMessage('');
                setError('An error occurred while submitting the form.');
                throw new Error('Network response was not ok ' + response.statusText);
            }


            setSuccessMessage('Check your email for verification details.');
            setError('');


        } catch (error) {
            setError('An error occurred while submitting the form.');
            setSuccessMessage('');
            console.error('Error:', error);
        }
    };

    return (
        <div className="popup-overlay">
            <div className="popup-content">
                <button className="close-button" onClick={onClose}>X</button>
                <img src={require(`../../assets/CarsImages/${car.image}`)} alt={car.name} className="popup-image" />
                <h3>{car.name}</h3>
                <p>{car.description}</p>
                <p>Price per day: ${car.pricePerDay}</p>
                <p>Start Unit: {startUnit}</p>
                <p>End Unit: {endUnit}</p>
                <p>Begin Date: {beginDate.toLocaleDateString()}</p>
                <p>End Date: {endDate.toLocaleDateString()}</p>
                <h2>Please fill up the form below.</h2>
                <input
                    className="inputBox"
                    type="text"
                    placeholder="First Name"
                    value={firstName}
                    onChange={(e) => setFirstName(e.target.value)}
                />
                <input
                    className="inputBox"
                    type="text"
                    placeholder="Last Name"
                    value={lastName}
                    onChange={(e) => setLastName(e.target.value)}
                />
                <input
                    className="inputBox"
                    type="email"
                    placeholder="Email"
                    value={email}
                    onChange={(e) => setEmail(e.target.value)}
                />
                {error && <p className="error">{error}</p>}
                {successMessage && <p className="success">{successMessage}</p>}
                <button id="submit-button" onClick={handleSubmit}>Submit</button>
            </div>
        </div>
    );
};

export default DetailsPopup;
