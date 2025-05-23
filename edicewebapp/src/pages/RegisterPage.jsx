import {useTitle} from "../Hooks/useTitle.js";
import RegisterForm from "../components/RegisterForm/RegisterForm.jsx";

export default function RegisterPage() {
    useTitle('eDice - Регистрация')

    return (
        <>
            <div className='register-page'>
                <RegisterForm></RegisterForm>
            </div>
        </>
    )
}