import React, { useEffect, useState } from 'react';
import { useLocation, useNavigate } from 'react-router-dom';

const Verify = () => {
    const location = useLocation();
    const navigate = useNavigate();
    const [verificationStatus, setVerificationStatus] = useState('Verifying...');
    const [error, setError] = useState(null);

    useEffect(() => {
        const params = new URLSearchParams(location.search);
        const token = params.get('token');
        const carId = params.get('carId');

        if (token && carId) {
            console.log("Verifying token and carId:", token, carId);
            fetch(`http://localhost:5146/api/Happy/Verify?token=${token}&carId=${carId}`, {
                method: 'GET',
            })
                .then(response => {
                    if (!response.ok) {
                        throw new Error('Network response was not ok ' + response.statusText);
                    }
                    return response.json();
                })
                .then(data => {
                    console.log("Verification result:", data);
                    if (data.success) {
                        navigate('/success');
                    } else {
                        navigate('/error');
                    }
                })
                .catch(error => {
                    console.error('Error:', error);
                    setError('Verification failed. Please try again later.');
                    navigate('/error');
                });
        } else {
            setError('Invalid verification link.');
            navigate('/error');
        }
    }, [location.search, navigate]); // Używaj location.search i navigate jako zależności

    return (
        <div>
            <h1>Verification Status</h1>
            {error ? <p>{error}</p> : <p>{verificationStatus}</p>}
        </div>
    );
};

export default Verify;