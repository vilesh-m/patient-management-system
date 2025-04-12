import React, { useState, useEffect } from 'react';
import './App.css';
import AuthCard from './components/AuthCard';
import PatientCard from './components/PatientCard';

function App() {
  const [isAuthenticated, setIsAuthenticated] = useState(false);

  useEffect(() => {
    const token = localStorage.getItem('token');
    if (token) {
      setIsAuthenticated(true);
    }
  }, []);

  const handleLoginSuccess = () => {
    setIsAuthenticated(true);
  };

  const handlelogoutSuccess = () => {
    setIsAuthenticated(false);
  };

  return (
    <div>
      <div className="container">
        <header className="mb-4 text-center">
          <h1 className="fw-bold text-dark">Patient Management System</h1>
          <p className="text-secondary">Manage patient records securely</p>
        </header>

        <main>
          <div className="mb-4">
          <AuthCard onSuccesfulLogin={handleLoginSuccess} onSuccesfulLogout={handlelogoutSuccess} />
          </div>
          {isAuthenticated && (
            <PatientCard />
          )
          }
        </main>
      </div>
    </div>
  );
}

export default App;
