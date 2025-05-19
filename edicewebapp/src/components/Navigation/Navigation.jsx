import './Navigation.css'
import NavigationList from "./NavigationList.jsx";
import {MenuItems} from "./NavigationItemsData.js";

function Navigation() {
    return (
        <div className='navigation-container'>
            <NavigationList items={MenuItems}/>
        </div>
    )
}

export default Navigation