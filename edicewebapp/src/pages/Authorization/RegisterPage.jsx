import {useTitle} from "../../Hooks/useTitle.js";
import RegisterForm from "../../components/Authorization/RegisterForm.jsx";

export default function RegisterPage() {
    useTitle('eDice - Создать аккаунт')

    return (
        <div className='authorization-container'>
            <RegisterForm></RegisterForm>
        </div>
    )
}