import './Button.css'

function Button({children, className, color, ...props}) {
    return (
        <button className={[className, color].join(' ')} {...props}>{children}</button>
    )
}

export default Button