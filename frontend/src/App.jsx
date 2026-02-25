import { BrowserRouter, Route, Routes, Link } from 'react-router-dom';
import ContactListPage from './pages/ContactListPage';
import ContactFormPage from './pages/ContactFormPage';
import LoginForm from './components/LoginForm';
import { useState } from 'react';


function App() {
  const [isLoggedIn, setIsLoggedIn] = useState(!!localStorage.getItem('token'));

  const handleLoginSuccess = () => {
    setIsLoggedIn(true);
  }
  const handleLogout = () => {
    localStorage.removeItem('token');
    setIsLoggedIn(false);
  }

  return (
    <BrowserRouter>
      <div>
        <div>
          {!isLoggedIn ? <LoginForm onLoginSuccess={handleLoginSuccess} />
            :
            <div>
              <span style={{ marginRight: '10px', fontWeight: 'bold' }}>Zalogowano pomyślnie!</span>
              <button onClick={handleLogout} style={{ color: 'red', cursor: 'pointer' }}>Wyloguj</button>
            </div>
          }
        </div>
        <nav>
          <Link to="/" style={{ color: 'blue' }}>Lista Kontaktów</Link>
          {isLoggedIn && <Link to="/add" style={{ color: 'green' }}>Dodaj Nowy</Link>}
        </nav>
        <Routes>
          <Route path="/" element={<ContactListPage isLoggedIn={isLoggedIn} />} />
          <Route path="/add" element={<ContactFormPage />} />
          <Route path="/edit/:id" element={<ContactFormPage />} />
        </Routes>
      </div>
    </BrowserRouter>
  )
}

export default App;