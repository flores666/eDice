function NavigationList({items}) {
    function isActive(href) {
        if (href === '/') return location.pathname === href ? 'active' : '';
        return location.pathname.startsWith(href) ? 'active' : ''
    }
    
    return (
        <ul>
            {items.map((item, index) => (
                <li key={index} className={isActive(item.href)}>
                    <a href={item.href}>
                        <span>
                            <img src={item.image} alt='icon'/>
                        </span>
                        <span>{item.text}</span>
                    </a>
                </li>
            ))}
        </ul>
    )
}

export default NavigationList