import { useState, useEffect } from 'react';
import axios from 'axios';
import { Pie } from 'react-chartjs-2';
import { Container } from 'react-bootstrap';
import {
    Chart as ChartJS,
    ArcElement,
    Tooltip,
    Legend,
    Title
} from 'chart.js';

ChartJS.register(ArcElement, Tooltip, Legend, Title);

export default function Statistics() {
    const [chartData, setChartData] = useState({ labels: [], datasets: [] });

    useEffect(() => {
        const token = localStorage.getItem('google_token');
        const config = {
            headers: { Authorization: `Bearer ${token}` }
        };

        Promise.all([
            axios.get('http://localhost:5010/api/categories', config),
            axios.get('http://localhost:5010/api/products', config)
        ]).then(([categoriesRes, productsRes]) => {
            const categories = categoriesRes.data;
            const products = productsRes.data;

            const categoryCounts = categories.map(category => {
                // Шукаємо всі товари, які мають такий самий categoryId
                return products.filter(p => p.categoryId === category.id).length;
            });

            // Формуємо дані для кругової діаграми
            setChartData({
                labels: categories.map(c => c.name), 
                datasets: [
                    {
                        label: 'Кількість товарів',
                        data: categoryCounts,
                        backgroundColor: [
                            'rgba(255, 99, 132, 0.7)',
                            'rgba(54, 162, 235, 0.7)',
                            'rgba(255, 206, 86, 0.7)',
                            'rgba(75, 192, 192, 0.7)',
                            'rgba(153, 102, 255, 0.7)',
                        ],
                        borderColor: [
                            'rgba(255, 99, 132, 1)',
                            'rgba(54, 162, 235, 1)',
                            'rgba(255, 206, 86, 1)',
                            'rgba(75, 192, 192, 1)',
                            'rgba(153, 102, 255, 1)',
                        ],
                        borderWidth: 1,
                    }
                ]
            });
        }).catch(error => console.error("Помилка завантаження даних:", error));
    }, []);

    const options = {
        responsive: true,
        maintainAspectRatio: false,
        plugins: {
            legend: { position: 'right' },
            title: { display: true, text: 'Розподіл асортименту за категоріями', font: { size: 18 } },
        },
    };

    return (
        <Container className="mt-5 mb-5">
            <h2 className="mb-4 text-center">Аналітика каталогу</h2>
            <div className="p-4 bg-white shadow-sm rounded border d-flex justify-content-center" style={{ height: '500px' }}>
                {chartData.labels.length > 0 ? (
                    <Pie options={options} data={chartData} />
                ) : (
                    <p className="text-muted text-center align-self-center">Завантаження статистики...</p>
                )}
            </div>
        </Container>
    );
}