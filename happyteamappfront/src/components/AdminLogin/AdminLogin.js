import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import './AdminLogin.css';

const AdminLogin = ({ setToken }) => {
    const [login, setLogin] = useState('');
    const [password, setPassword] = useState('');
    const [error, setError] = useState('');
    const navigate = useNavigate();

    const handleLogin = async () => {
        try {
            const response = await fetch(`http://localhost:5146/api/Happy/AdminLogin?login=${login}&password=${password}`, {
                method: 'GET'
            });
            if (!response.ok) {
                throw new Error('Network response was not ok ' + response.statusText);
            }
            const data = await response.json();
            setToken(data.token);
            setError('');
            navigate('/admin-panel');
        } catch (error) {
            console.error('Error during login:', error);
            setError('Invalid login or password');
        }
    };
    return (
        <div className="admin-container">
            <h1>Admin Login</h1>
            <input type="text" placeholder="Login" value={login} onChange={(e) => setLogin(e.target.value)} />
            <input type="password" placeholder="Password" value={password} onChange={(e) => setPassword(e.target.value)} />
            <button onClick={handleLogin}>Login</button>
            {error && <p>{error}</p>}
        </div>
    );
};

export default AdminLogin;