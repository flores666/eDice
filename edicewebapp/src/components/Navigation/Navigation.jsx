import './Navigation.css'
import NavigationList from "./NavigationList.jsx";

function Navigation() {
    let menuItems = [
        {text: 'Главная', href: '/', image: 'src/assets/templateIcon.svg'},
        {text: 'Лобби', href: '/lobby', image: 'src/assets/templateIcon.svg'},
        {text: 'Библиотека', href: '/library', image: 'src/assets/templateIcon.svg'},
        {text: 'Конструктор', href: '/constructor', image: 'src/assets/templateIcon.svg'},
    ]

    return (
        <div className='navigation-container'>
            <NavigationList items={menuItems}/>
        </div>
    )
}

export default Navigation