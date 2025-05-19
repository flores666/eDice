import './Menu.css'
import MiniProfile from "../MiniProfile/MiniProfile.jsx";
import Button from "../Button/Button.jsx";
import {useState} from "react";

function Menu() {
    const [inputValue, setInputValue] = useState('content value')
    
    const connectToLobby = () => {
    }

    return (
        <div className='menu-container'>
            <div className='container'>
                <a className='logo' href='/'>
                    <img src='/public/logo_big.svg' alt='eDice'/>
                    <span>eDice</span>
                </a>
                <div className='play'>
                    <input placeholder='Введите ключ' onChange={(e) => setInputValue(e.target.value)}/>
                    <Button color={'black'} onClick={connectToLobby}>Подключиться</Button>
                </div>
                <MiniProfile></MiniProfile>
            </div>
        </div>
    )
}

export default Menu