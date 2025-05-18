import './MiniProfile.css'
import Button from "../Button/Button.jsx";

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
                <Button color='gray'>Войти</Button>
                <Button color='white'>Зарегистрироваться</Button>
            </div>
        )
    }

    if (isAuthenticated) return authenticatedResult()
    return notAuthenticatedResult()
}

export default MiniProfile