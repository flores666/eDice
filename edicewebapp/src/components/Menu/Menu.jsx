import './Menu.css'
import MiniProfile from "../MiniProfile/MiniProfile.jsx";
import Button from "../Button/Button.jsx";

function Menu() {
    let userInput = ''
    
    const connectToLobby = () => {
    }
    
    const changeInput = (e) => {
        userInput = e.target.value
    }

    return (
        <div className='menu-container'>
            <div className='container'>
                <a className='logo' href='/'>
                    <img src='/public/logo_big.svg' alt='eDice'/>
                    <span>eDice</span>
                </a>
                <div className='play'>
                    <input placeholder='Введите ключ' onChange={changeInput}/>
                    <Button className='btn' color={'black'} onClick={connectToLobby}>Подключиться</Button>
                </div>
                <MiniProfile></MiniProfile>
            </div>
        </div>
    )
}

export default Menu