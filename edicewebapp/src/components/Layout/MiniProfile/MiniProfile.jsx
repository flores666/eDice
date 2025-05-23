import './MiniProfile.css'
import Button from "../../Button/Button.jsx";
import {Link} from "react-router-dom";

function MiniProfile() {
    let isAuthenticated = false
    let userName = 'P.Diddler'
    let profilePictureUrl = ''

    let authenticatedResult = () => {
        return (
            <a href='/user' className='mini-profile-container'>
                <span>{userName}</span>
                {
                    profilePictureUrl === '' 
                        ? (<div className='no-pic'></div>)
                        : (<img src={profilePictureUrl} alt='no photo'/>)
                }
            </a>
        )
    }

    let notAuthenticatedResult = () => {
        return (
            <div className='mini-profile-container buttons'>
                <Link to='/login'><Button color='gray'>Войти</Button></Link>
                <Link to='/register'><Button color='white'>Регистрация</Button></Link>
            </div>
        )
    }

    if (isAuthenticated) return authenticatedResult()
    return notAuthenticatedResult()
}

export default MiniProfile