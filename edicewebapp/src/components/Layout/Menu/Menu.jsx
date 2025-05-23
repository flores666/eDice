import './Menu.css'
import MiniProfile from "../MiniProfile/MiniProfile.jsx";
import Button from "/src/components/Button/Button.jsx";
import useInput from "/src/Hooks/useInput.js";
import {Link} from "react-router-dom";

function Menu() {
    let input = useInput('');
    
    const connectToLobby = () => {
        console.log(input.value)
    }

    return (
        <div className='menu-container'>
            <div className='container'>
                <Link className='logo' to='/'>
                    <img src='/public/logo_big.svg' alt='eDice'/>
                    <span>eDice</span>
                </Link>
                <div className='play'>
                    <input className='secondary' placeholder='Введите ключ' {...input}/>
                    <Button color={'black'} onClick={connectToLobby}>Подключиться</Button>
                </div>
                <MiniProfile></MiniProfile>
            </div>
        </div>
    )
}

export default Menu