import React, { useState } from 'react';
import './AdminPanel.css';

const AdminPanel = ({ token }) => {
    const [message, setMessage] = useState('');

    const handleAction = async (action) => {
        try {
            let url;
            switch (action) {
                case 'CleanUnVerified':
                    url = `http://localhost:5146/api/Happy/CleanUnVerified?token=${token}`;
                    break;
                case 'CleanOldData':
                    url = `http://localhost:5146/api/Happy/CleanOldData?token=${token}`;
                    break;
                case 'CarReturned':
                    const carId = prompt('Enter Car ID:');
                    url = `http://localhost:5146/api/Happy/CarReturned?token=${token}&CarId=${carId}`;
                    break;
                default:
                    return;
            }

            const response = await fetch(url, {
                method: 'GET'
            });

            if (!response.ok) {
                throw new Error('Network response was not ok ' + response.statusText);
            }

            setMessage('Action completed successfully');
        } catch (error) {
            console.error('Error during action:', error);
            setMessage('Action failed');
        }
    };

    return (
        <div className="admin-container">
            <h1>Admin Panel</h1>
            <button onClick={() => handleAction('CleanUnVerified')}>Clean Unverified Data</button>
            <button onClick={() => handleAction('CleanOldData')}>Clean Old Data</button>
            <button onClick={() => handleAction('CarReturned')}>Car Returned</button>

            {message && <p>{message}</p>}
        </div>
    );
};

export default AdminPanel;