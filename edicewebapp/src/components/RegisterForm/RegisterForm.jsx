import { useForm } from 'react-hook-form';
import { yupResolver } from '@hookform/resolvers/yup';
import * as yup from 'yup';
import './RegisterForm.css';
import Button from "../Button/Button.jsx";

const schema = yup.object({
    email: yup.string().email('Неверный email').required('Email обязателен'),
    userName: yup.string().required('Имя обязательно'),
    password: yup.string().min(6, 'Минимум 6 символов').required('Пароль обязателен'),
    passwordConfirm: yup
        .string()
        .oneOf([yup.ref('password')], 'Пароли не совпадают')
        .required('Подтвердите пароль'),
});

export default function RegisterForm() {
    const {
        register,
        handleSubmit,
        formState: { errors },
    } = useForm({
        resolver: yupResolver(schema),
    });

    const onSubmit = (data) => {
        console.log('Форма отправлена:', data);
    };

    return (
        <form onSubmit={handleSubmit(onSubmit)} className='card'>
            <span className='title'>
                <h1>Регистрация</h1>
            </span>
            <div className='form-group'>
                <label htmlFor='email' data-required='true'>Адрес электронной почты</label>
                <input id='email' placeholder='Введите email' {...register('email')} />
                <small>Будет использоваться как логин</small>
                {errors.email && <p className="error">{errors.email.message}</p>}
            </div>

            <div className='form-group'>
                <label htmlFor='userName' data-required='true'>Отображаемое имя</label>
                <input id='userName' placeholder='Введите никнейм' {...register('userName')} />
                <small>Можно поменять в любой момент</small>
                {errors.userName && <p className="error">{errors.userName.message}</p>}
            </div>

            <div className='form-group'>
                <label htmlFor='password' data-required='true'>Пароль</label>
                <input type='password' id='password' placeholder='Введите пароль' {...register('password')} />
                {errors.password && <p className="error">{errors.password.message}</p>}
            </div>

            <div className='form-group'>
                <label htmlFor='passwordConfirm' data-required='true'>Повторите пароль</label>
                <input type='password' id='passwordConfirm' placeholder='Введите пароль' {...register('passwordConfirm')} />
                {errors.passwordConfirm && <p className="error">{errors.passwordConfirm.message}</p>}
            </div>

            <Button type='submit' color='white'>Зарегистрироваться</Button>
        </form>
    );
}
