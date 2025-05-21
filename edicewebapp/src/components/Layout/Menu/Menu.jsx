import './Menu.css'
import MiniProfile from "../MiniProfile/MiniProfile.jsx";
import Button from "/src/components/Button/Button.jsx";
import useInput from "/src/Hooks/useInput.js";

function Menu() {
    let input = useInput('');
    
    const connectToLobby = () => {
        console.log(input.value)
    }

    return (
        <div className='menu-container'>
            <div className='container'>
                <a className='logo' href='/'>
                    <img src='/public/logo_big.svg' alt='eDice'/>
                    <span>eDice</span>
                </a>
                <div className='play'>
                    <input placeholder='Введите ключ' {...input}/>
                    <Button color={'black'} onClick={connectToLobby}>Подключиться</Button>
                </div>
                <MiniProfile></MiniProfile>
            </div>
        </div>
    )
}

export default Menu