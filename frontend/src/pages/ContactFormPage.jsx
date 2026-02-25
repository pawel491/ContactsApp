import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { API_URL } from '../GlobalConst';


function ContactFormPage() {
    const { id } = useParams();
    const navigate = useNavigate();
    const isEditMode = Boolean(id);

    const [categories, setCategories] = useState([]);

    const [formData, setFormData] = useState({
        name: '',
        surname: '',
        email: '',
        password: '',
        phoneNumber: '',
        dateOfBirth: '',
        categoryName: '',
        subcategoryName: '',
        customSubcategory: ''
    });

    useEffect(() => {
        const token = localStorage.getItem('token');
        const requestOptions = {
            headers: {
                'Authorization': `Bearer ${token}`
            }
        };
        // fetch all possible categories(and their subcategories)
        fetch(`${API_URL}/category`, requestOptions)
            .then(response => {
                // if 4xx or 5xx then stop and throw
                if (!response.ok) throw new Error(response.status);
                return response.json();
            })
            .then(data => setCategories(data))
            .catch(err => {
                if (err.message === '401') {
                    alert("Brak uprawnień. Zaloguj się.");
                } else {
                    console.error("Błąd:", err);
                }
            });

        // fetch data of edited contact
        if (isEditMode) {
            fetch(`${API_URL}/contact/${id}`, requestOptions)
                .then(response => {
                    // if 4xx or 5xx then stop and throw
                    if (!response.ok) throw new Error(response.status);
                    return response.json();
                })
                .then(data => {
                    setFormData({
                        name: data.name || '',
                        surname: data.surname || '',
                        email: data.email || '',
                        password: '', // never fetched
                        phoneNumber: data.phoneNumber || '',
                        dateOfBirth: data.dateOfBirth || '',
                        categoryName: data.categoryName || '',
                        subcategoryName: data.subcategoryName || '',
                        customSubcategory: data.customSubcategory || ''
                    });
                })
                .catch(err => {
                    if (err.message === '401') {
                        alert("Brak uprawnień. Zaloguj się.");
                    } else {
                        alert("Błąd:", err);
                    }
                });
        }
    }, [id, isEditMode]);

    // update form state after each change
    const handleChange = (e) => {
        const { name, value } = e.target;

        setFormData(prev => {
            const newData = { ...prev, [name]: value };

            // if categoryName changed then reset those fields
            if (name === 'categoryName') {
                newData.subcategoryName = '';
                newData.customSubcategory = '';
            }

            return newData;
        });
    };

    const handleSubmit = async (e) => {
        e.preventDefault();

        if (!isEditMode && formData.password.length < 8) {
            alert("Błąd: Hasło musi składać się z minimum 8 znaków!");
            return; // so invalid request is never sent to backend
        }

        const url = isEditMode ? `${API_URL}/contact/${id}` : `${API_URL}/contact`;
        const method = isEditMode ? "PUT" : "POST";
        const token = localStorage.getItem('token');

        const response = await fetch(url, {
            method: method,
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${token}`
            },
            body: JSON.stringify(formData)
        })

        if (response.ok) {
            navigate('/');
        } else if (response.status === 401) {
            alert("Brak uprawnień. Zaloguj się.");
        } else {
            const errorMessage = await response.text();
            alert(`Błąd zapisu: ${errorMessage}`);
            console.error("Szczegóły błędu:", errorMessage);
        }
    }
    const selectedCategory = categories.find(c => c.name === formData.categoryName);
    const hasSubcategories = selectedCategory && selectedCategory.subcategories.length > 0;
    const isCustom = selectedCategory?.name.toLowerCase() === 'inny';

    return (
        <div>
            <h2>{isEditMode ? 'Edytuj Kontakt' : 'Dodaj Kontakt'}</h2>

            <form onSubmit={handleSubmit} style={{ display: 'flex', flexDirection: 'column', width: '300px', gap: '10px' }}>
                <input type="text" name="name" value={formData.name} onChange={handleChange} placeholder="Imię" required />
                <input type="text" name="surname" value={formData.surname} onChange={handleChange} placeholder="Nazwisko" required />
                <input type="email" name="email" value={formData.email} onChange={handleChange} placeholder="E-mail" required />

                {/* Hide in edit mode */}
                {!isEditMode && (
                    <input type="password" name="password" minLength="8" value={formData.password} onChange={handleChange} placeholder="Hasło" required />
                )}

                <input type="text" name="phoneNumber" value={formData.phoneNumber} onChange={handleChange} placeholder="Telefon" required />
                <input type="date" name="dateOfBirth" value={formData.dateOfBirth} onChange={handleChange} required />

                <select name="categoryName" value={formData.categoryName} onChange={handleChange} required>
                    <option value="" disabled>-- Wybierz kategorię --</option>
                    {categories.map(cat => (
                        <option key={cat.id} value={cat.name}>{cat.name}</option>
                    ))}
                </select>

                {hasSubcategories && (
                    <select name="subcategoryName" value={formData.subcategoryName} onChange={handleChange} required>
                        <option value="" disabled>-- Wybierz podkategorię --</option>
                        {selectedCategory.subcategories.map(sub => (
                            <option key={sub.id} value={sub.name}>{sub.name}</option>
                        ))}
                    </select>
                )}

                {isCustom && (
                    <input
                        type="text"
                        name="customSubcategory"
                        value={formData.customSubcategory}
                        onChange={handleChange}
                        placeholder="Wpisz własną kategorię"
                        required
                    />
                )}

                <button type="submit">Zapisz</button>
            </form>
        </div>
    );
}

export default ContactFormPage;