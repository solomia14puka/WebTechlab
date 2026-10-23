import { useState, useEffect } from 'react';
import axios from 'axios';
import { Container, Navbar, Nav, Card, Row, Col, Form } from 'react-bootstrap';
import { BrowserRouter as Router, Routes, Route, Link } from 'react-router-dom';
import ReactMarkdown from 'react-markdown';

// Каталог (С6)
function Catalog() {
    const [products, setProducts] = useState([]);

    useEffect(() => {
        axios.get('http://localhost:5010/api/products')
            .then(response => setProducts(response.data))
            .catch(error => console.error("Помилка:", error));
    }, []);

    return (
        <Container className="mt-4">
            <h1 className="mb-4 text-center">Наші товари</h1>
            <Row xs={1} md={2} lg={3} className="g-4">
                {products.map((product) => (
                    <Col key={product.id}>
                        <Card className="h-100 shadow-sm">
                            <Card.Img variant="top" src={product.imageUrl || 'https://via.placeholder.com/300x200'} style={{ height: '200px', objectFit: 'cover' }} />
                            <Card.Body>
                                <Card.Title>{product.name}</Card.Title>
                                <Card.Text>{product.description}</Card.Text>
                                <h4 className="text-primary">{product.price} грн</h4>
                            </Card.Body>
                        </Card>
                    </Col>
                ))}
            </Row>
        </Container>
    );
}

// Редактор (C12)
function Editor() {
    const [text, setText] = useState(() => {
        return localStorage.getItem('my-editor-text') || '# Назва товару\n\nОпис вашого товару...\n\n* Характеристика 1\n* Характеристика 2';
    });

    useEffect(() => {
        localStorage.setItem('my-editor-text', text);
    }, [text]);

    return (
        <Container className="mt-4">
            <h2 className="mb-3">Редактор</h2>
            <Row>
                <Col md={6}>
                    <h5 className="text-muted">Введення</h5>
                    <Form.Control
                        as="textarea"
                        rows={15}
                        value={text}
                        onChange={(e) => setText(e.target.value)}
                        className="shadow-sm font-monospace"
                    />
                </Col>
                <Col md={6}>
                    <h5 className="text-muted">Готовий вигляд</h5>
                    <div className="p-3 border rounded shadow-sm bg-white" style={{ minHeight: '360px' }}>
                        <ReactMarkdown>{text}</ReactMarkdown>
                    </div>
                </Col>
            </Row>
        </Container>
    );
}

function App() {
    return (
        <Router>
            <Navbar bg="dark" variant="dark" sticky="top" className="px-4 shadow">
                <Navbar.Brand as={Link} to="/">WebStore</Navbar.Brand>
                <Nav className="me-auto">
                    <Nav.Link as={Link} to="/">Каталог</Nav.Link>
                    <Nav.Link as={Link} to="/editor">Редактор</Nav.Link>
                </Nav>
            </Navbar>
            <Routes>
                <Route path="/" element={<Catalog />} />
                <Route path="/editor" element={<Editor />} />
            </Routes>
        </Router>
    );
}

export default App;