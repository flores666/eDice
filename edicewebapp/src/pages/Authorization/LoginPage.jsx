import {useTitle} from "../../Hooks/useTitle.js";
import LoginForm from "../../components/Authorization/LoginForm.jsx";

export default function LoginPage() {
    useTitle('eDice - Войти в аккаунт')

    return (
        <div className='authorization-container'>
            <LoginForm></LoginForm>
        </div>
    )
}