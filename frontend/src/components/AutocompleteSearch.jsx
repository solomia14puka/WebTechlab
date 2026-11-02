import axios from 'axios';
import { useState, useEffect } from 'react';

export default function AutocompleteSearch() {
    const [query, setQuery] = useState('');
    const [results, setResults] = useState([]);
    const [isOpen, setIsOpen] = useState(false);

    useEffect(() => {
        const delayDebounceFn = setTimeout(async () => {
            if (query.trim().length < 3) {
                setResults([]);
                setIsOpen(false);
                return;
            }

            try {
                const response = await axios.get(`https://api.render.com/deploy/srv-dbkgc0m1egvs7394sudg?key=DI--hvjFpcY/api/products/search?q=${query}`);
                setResults(response.data);
                setIsOpen(true);
            } catch (error) {
                console.error("Помилка пошуку:", error);
            }
        }, 300);

        return () => clearTimeout(delayDebounceFn);
    }, [query]);

    return (
        <div className="position-relative w-100" style={{ maxWidth: '400px' }}>
            <input
                type="text"
                className="form-control"
                placeholder="Пошук товарів (від 3 символів)..."
                value={query}
                onChange={(e) => setQuery(e.target.value)}
                onBlur={() => setTimeout(() => setIsOpen(false), 200)} // Ховаємо список при втраті фокусу
                onFocus={() => { if (results.length > 0) setIsOpen(true) }}
            />

            {/* Випадаючий список результатів */}
            {isOpen && (
                <ul className="list-group position-absolute w-100 shadow-sm mt-1" style={{ zIndex: 1050 }}>
                    {results.length > 0 ? (
                        results.map(product => (
                            <li key={product.id} className="list-group-item list-group-item-action cursor-pointer">
                                {product.name} — {product.price} грн
                            </li>
                        ))
                    ) : (
                        <li className="list-group-item text-muted">Нічого не знайдено</li>
                    )}
                </ul>
            )}
        </div>
    );
}