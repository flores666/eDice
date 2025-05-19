import {useState} from "react";

export default function useInput(defaultValue = '') {
    let [value, setValue] = useState(defaultValue)
    
    return {
        value,
        onChange: (e) => setValue(e.target.value)
    }
} 