import {Link, useLocation} from "react-router-dom";

function NavigationList({items}) {
    const location = useLocation()
    
    function isActive(href) {
        if (href === '/') return location.pathname === href ? 'active' : '';
        return location.pathname.startsWith(href) ? 'active' : ''
    }
    
    return (
        <ul>
            {items.map((item, index) => (
                <li key={index} className={isActive(item.href)}>
                    <Link to={item.href}>
                        <span>
                            <img src={item.image} alt='icon'/>
                        </span>
                        <span>{item.text}</span>
                    </Link>
                </li>
            ))}
        </ul>
    )
}

export default NavigationList