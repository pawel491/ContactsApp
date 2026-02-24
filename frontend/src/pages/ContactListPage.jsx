import { useState, useEffect } from 'react';
import { API_URL } from '../constValues';



function ContactListPage() {
    const [contacts, setContacts] = useState([]);
    const [isLoading, setIsLoading] = useState(true);
    useEffect(() => {
        fetchContacts();
    }, []);

    const fetchContacts = async () => {
        try {
            const response = await fetch(`${API_URL}/contact`);
            if (!response.ok) {
                throw new Error(`HTTP Error: ${response.status}`);
            }

            const data = await response.json();
            console.log(data);
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
                            {/* <th>Kategoria</th> */}
                        </tr>
                    </thead>
                    <tbody>
                        {contacts.map((contact) => (
                            <tr key={contact.email}>
                                <td>{contact.name}</td>
                                <td>{contact.surname}</td>
                                <td>{contact.email}</td>
                                <td>{contact.phoneNumber}</td>
                                {/* <td>{contact.categoryName || contact.category}</td> */}
                            </tr>
                        ))}
                    </tbody>
                </table>
            )}
        </div>
    );
}
export default ContactListPage;