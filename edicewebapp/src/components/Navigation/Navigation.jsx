import './Navigation.css'
import IconList from "../IconList.jsx";

function Navigation() {
    let menuItems = [
        {text: 'Главная', href: '/', image: 'src/assets/templateIcon.svg'},
        {text: 'Лобби', href: '/lobby', image: 'src/assets/templateIcon.svg'},
        {text: 'Библиотека', href: '/library', image: 'src/assets/templateIcon.svg'},
        {text: 'Конструктор', href: '/constructor', image: 'src/assets/templateIcon.svg'},
    ]

    return (
        <div className='navigation-container'>
            <IconList items={menuItems}/>
        </div>
    )
}

export default Navigation