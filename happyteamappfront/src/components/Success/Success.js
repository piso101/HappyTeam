import React from 'react';
import './Success.css';

const Success = () => {
    return (
        <div className="success-container">
            <h1>Verification Successful</h1>
            <p>Your verification was successful.</p>
            <button className="success-button" onClick={() => window.location.href = '/'}>Go to Homepage</button>
        </div>
    );
};

export default Success;