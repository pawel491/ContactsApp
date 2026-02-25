import { useState, useEffect } from 'react';
import { API_URL } from '../GlobalConst';
import { useNavigate } from 'react-router-dom';



function ContactListPage({ isLoggedIn }) {
    const [contacts, setContacts] = useState([]);
    const [isLoading, setIsLoading] = useState(true);
    const navigate = useNavigate();

    useEffect(() => {
        fetchContacts();
    }, []);

    const handleDelete = async (id) => {
        if (!window.confirm("Czy na pewno chcesz usunąć ten kontakt?")) return;

        try {
            const token = localStorage.getItem('token');
            const response = await fetch(`${API_URL}/contact/${id}`, {
                method: 'DELETE',
                headers: {
                    'Authorization': `Bearer ${token}`
                }
            });

            if (response.ok) {
                setContacts(prevContacts => prevContacts.filter(c => c.id !== id));
            } else {
                alert("Nie udało się usunąć kontaktu.");
            }
        } catch (err) {
            console.error("Błąd podczas usuwania:", err);
        }
    }

    const fetchContacts = async () => {
        try {
            const response = await fetch(`${API_URL}/contact`);
            if (!response.ok) {
                throw new Error(`HTTP Error: ${response.status}`);
            }

            const data = await response.json();
            setContacts(data);
        } catch (err) {
            console.error("Error fetching contacts:", err);
        } finally {
            setIsLoading(false);
        }
    };

    if (isLoading) return <h2>Loading contacts...</h2>;

    return (
        <div style={{ padding: '20px', fontFamily: 'sans-serif' }}>
            <h1>Contacts Manager</h1>

            {contacts.length === 0 ? (
                <p>Brak kontaktów.</p>
            ) : (
                <table border="1" cellPadding="10" style={{ borderCollapse: 'collapse', width: '100%', maxWidth: '800px' }}>
                    <thead>
                        <tr style={{ backgroundColor: '#f0f0f0', textAlign: 'left' }}>
                            <th>Imię</th>
                            <th>Nazwisko</th>
                            <th>Email</th>
                            <th>Numer Telefonu</th>
                            <th>Akcje</th>
                        </tr>
                    </thead>
                    <tbody>
                        {contacts.map((contact) => (
                            <tr key={contact.id}>
                                <td>{contact.name}</td>
                                <td>{contact.surname}</td>
                                <td>{contact.email}</td>
                                <td>{contact.phoneNumber}</td>
                                <td>
                                    <button
                                        disabled={!isLoggedIn}
                                        onClick={() => navigate(`/edit/${contact.id}`)}
                                        style={{ backgroundColor: isLoggedIn ? 'blue' : 'gray', marginRight: '10px' }}
                                    >
                                        Edytuj
                                    </button>

                                    <button
                                        disabled={!isLoggedIn}
                                        onClick={() => handleDelete(contact.id)}
                                        style={{ backgroundColor: isLoggedIn ? 'red' : 'gray', color: 'white', border: 'none', padding: '5px 10px' }}
                                    >
                                        Usuń
                                    </button>
                                </td>
                            </tr>
                        ))}
                    </tbody>
                </table>
            )}
        </div>
    );
}
export default ContactListPage;