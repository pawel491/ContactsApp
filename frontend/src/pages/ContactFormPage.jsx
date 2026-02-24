import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { API_URL } from '../constValues';


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
        fetch(`${API_URL}/category`)
            .then(res => res.json())
            .then(data => setCategories(data))
            .catch(err => console.error("Błąd pobierania kategorii:", err))

        if (isEditMode) {
            fetch(`${API_URL}/contact/${id}`)
                .then(response => response.json())
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
                .catch(err => console.error("Error fetching contact:", err));
        }
    }, [id, isEditMode]);

    // update form state after each change
    const handleChange = (e) => {
        const { name, value } = e.target;

        setFormData(prev => {
            const newData = { ...prev, [name]: value };

            if (name === 'categoryName') {
                newData.subcategoryName = '';
                newData.customSubcategory = '';
            }

            return newData;
        });
    };

    const handleSubmit = async (e) => {
        e.preventDefault();

        const url = isEditMode ? `${API_URL}/contact/${id}` : `${API_URL}/contact`;
        const method = isEditMode ? "PUT" : "POST";

        const response = await fetch(url, {
            method: method,
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(formData)
        })

        if (response.ok) {
            navigate('/');
        } else {
            alert("Coś poszło nie tak!");
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
                    <input type="password" name="password" value={formData.password} onChange={handleChange} placeholder="Hasło" required />
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