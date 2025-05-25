import {useForm} from 'react-hook-form';
import {yupResolver} from '@hookform/resolvers/yup';
import * as yup from 'yup';
import './AuthorizationForm.css';
import Button from "../Button/Button.jsx";

const schema = yup.object({
    email: yup.string().email('Неверный email').required('Email обязателен'),
    userName: yup.string().required('Имя обязательно').min(2, 'Минимум 2 символа'),
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
                <h1>Создать аккаунт</h1>
            </span>
            <div className='form-group'>
                <div className='form-label-group'>
                    <label htmlFor='email' data-required='true'>Email</label>
                    {errors.email && <span className="error">{errors.email.message}</span>}
                </div>
                <input id='email' placeholder='Введите email' {...register('email')} />
                <small>Будет использоваться как логин</small>
            </div>

            <div className='form-group'>
                <div className='form-label-group'>
                    <label htmlFor='userName' data-required='true'>Отображаемое имя</label>
                    {errors.userName && <span className="error">{errors.userName.message}</span>}
                </div>
                <input id='userName' placeholder='Введите никнейм' {...register('userName')} />
                <small>Можно поменять в любой момент</small>
            </div>

            <div className='form-group'>
                <div className='form-label-group'>
                    <label htmlFor='password' data-required='true'>Пароль</label>
                    {errors.password && <span className="error">{errors.password.message}</span>}
                </div>
                <input type='password' id='password' placeholder='Введите пароль' {...register('password')} />
            </div>

            <div className='form-group'>
                <div className='form-label-group'>
                    <label htmlFor='passwordConfirm' data-required='true'>Повторите пароль</label>
                    {errors.passwordConfirm && <span className="error">{errors.passwordConfirm.message}</span>}
                </div>
                <input type='password' id='passwordConfirm'
                       placeholder='Введите пароль' {...register('passwordConfirm')} />
            </div>

            <Button type='submit' color='white'>Зарегистрироваться</Button>
        </form>
    );
}
