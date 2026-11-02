import { GoogleLogin } from '@react-oauth/google';
import { jwtDecode } from "jwt-decode";

export default function LoginButton() {
    return (
        <GoogleLogin
            onSuccess={credentialResponse => {
                const decodedToken = jwtDecode(credentialResponse.credential);
                console.log("Успішний логін:", decodedToken);
                // Зберігаємо токен для доступу до захищеного API
                localStorage.setItem('google_token', credentialResponse.credential);
                alert(`Вітаємо, ${decodedToken.name}!`);
            }}
            onError={() => {
                console.log('Помилка авторизації');
            }}
        />
    );
}