import './Button.css'

function Button({children, onClick, color, type = 'button'}) {
    return (
        <button className={color} type={type} onClick={onClick}>{children}</button>
    )
}

export default Button