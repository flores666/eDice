function IconList({items}) {
    return (
        <ul>
            {items.map((item, index) => (
                <li key={index}>
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

export default IconList