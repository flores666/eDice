import {useForm} from 'react-hook-form';
import {yupResolver} from '@hookform/resolvers/yup';
import * as yup from 'yup';
import './AuthorizationForm.css';
import Button from "../Button/Button.jsx";

const schema = yup.object({
    email: yup.string().email('Неверный email').required('Email обязателен'),
    password: yup.string().min(6, 'Минимум 6 символов').required('Пароль обязателен'),
});

export default function LoginForm() {
    const {
        register,
        handleSubmit,
        formState: {errors},
    } = useForm({
        resolver: yupResolver(schema),
    });

    const onSubmit = (data) => {
        console.log('Форма отправлена:', data);
    };

    return (
        <form onSubmit={handleSubmit(onSubmit)} className='card'>
            <span className='title'>
                <h1>Войти в аккаунт</h1>
            </span>
            <div className='form-group'>
                <div className='form-label-group'>
                    <label htmlFor='email' data-required='true'>Email</label>
                    {errors.email && <span className="error">{errors.email.message}</span>}
                </div>
                <input id='email' placeholder='Введите email' {...register('email')} />
            </div>
            
            <div className='form-group'>
                <div className='form-label-group'>
                    <label htmlFor='password' data-required='true'>Пароль</label>
                    {errors.password && <span className="error">{errors.password.message}</span>}
                </div>
                <input type='password' id='password' placeholder='Введите пароль' {...register('password')} />
            </div>

            <Button type='submit' color='white'>Войти</Button>
        </form>
    );
}
