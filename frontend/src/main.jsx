import 'bootstrap/dist/css/bootstrap.min.css';
import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import './index.css'
import App from './App.jsx'
import { GoogleOAuthProvider } from '@react-oauth/google';

createRoot(document.getElementById('root')).render(
    <StrictMode>
        <GoogleOAuthProvider clientId="82571943442-s1gevchga4fd5nt4etdojp3np27au9ud.apps.googleusercontent.com">
            <App />
        </GoogleOAuthProvider>
    </StrictMode>,
)
