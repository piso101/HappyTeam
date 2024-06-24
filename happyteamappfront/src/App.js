import React, { useState } from 'react';
import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import Navbar from './components/NavBar/Navbar';
import UnitsComponent from './components/UnitComboBoxes/UnitComboBoxes';
import Verify from './components/Verify/Verify';
import Success from './components/Success/Success';
import Error from './components/Error/Error';
import AdminLogin from './components/AdminLogin/AdminLogin';
import AdminPanel from './components/AdminPanel/AdminPanel';

import './index.css';
import './App.css';

function App() {
  const [token, setToken] = useState('');

  return (
    <Router>
      <div className="App">
        <Navbar />
        <div className="content">
          <Routes>
            <Route path="/" element={<UnitsComponent />} />
            <Route path="/verify" element={<Verify />} />
            <Route path="/success" element={<Success />} />
            <Route path="/error" element={<Error />} />
            <Route path="/admin" element={<AdminLogin setToken={setToken} />} />
            <Route path="/admin-panel" element={token ? <AdminPanel token={token} /> : <Navigate to="/admin" />} />
          </Routes>
        </div>
      </div>
    </Router>
  );
}

export default App;