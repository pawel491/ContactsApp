import { BrowserRouter, Route, Routes, Link } from 'react-router-dom';
import './App.css';
import ContactListPage from './pages/ContactListPage';
import ContactFormPage from './pages/ContactFormPage';


function App() {
  return (
    <BrowserRouter>
      <div>
        <nav>
          <Link to="/" style={{ color: 'blue' }}>Lista Kontaktów</Link>
          <Link to="/add" style={{ color: 'green' }}>Dodaj Nowy</Link>
        </nav>
        <Routes>
          <Route path="/" element={<ContactListPage />} />
          <Route path="/add" element={<ContactFormPage />} />
          <Route path="/edit/:id" element={<ContactFormPage />} />
        </Routes>
      </div>
    </BrowserRouter>
  )
}

export default App;