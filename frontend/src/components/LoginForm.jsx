import { useState } from 'react';
import { API_URL } from '../GlobalConst';

// Jako "props" przyjmujemy funkcję, która odpali się po udanym logowaniu
function LoginForm({ onLoginSuccess }) {
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [error, setError] = useState(null);

    const handleSubmit = async (e) => {
        e.preventDefault();
        setError(null);

        try {
            const response = await fetch(`${API_URL}/auth/login`, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ email, password })
            });

            if (response.ok) {
                const data = await response.json();

                // Save token to local storage
                localStorage.setItem('token', data.token);
                onLoginSuccess();
            } else {
                setError('Nieprawidłowy e-mail lub hasło.');
            }
        } catch (err) {
            setError('Błąd połączenia z serwerem.');
        }
    };

    return (
        <div style={{ border: '1px solid #ccc', padding: '15px', marginBottom: '20px', maxWidth: '300px', backgroundColor: '#f9f9f9' }}>
            <h3 style={{ marginTop: 0 }}>Panel logowania</h3>

            {error && <p style={{ color: 'red', fontSize: '14px', margin: '5px 0' }}>{error}</p>}

            <form onSubmit={handleSubmit} style={{ display: 'flex', flexDirection: 'column', gap: '10px' }}>
                <input
                    type="email"
                    placeholder="E-mail"
                    value={email}
                    onChange={(e) => setEmail(e.target.value)}
                    required
                />
                <input
                    type="password"
                    placeholder="Hasło"
                    value={password}
                    onChange={(e) => setPassword(e.target.value)}
                    required
                />
                <button type="submit" style={{ backgroundColor: 'black', color: 'white', padding: '8px', cursor: 'pointer' }}>
                    Zaloguj się
                </button>
            </form>
        </div>
    );
}

export default LoginForm;