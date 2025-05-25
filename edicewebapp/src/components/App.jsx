import Navigation from "./Layout/Navigation/Navigation.jsx";
import Menu from "./Layout/Menu/Menu.jsx";
import Container from "./Layout/Container/Container.jsx";
import {Route, Routes} from "react-router-dom";

import HomePage from "../pages/HomePage.jsx";
import LobbyPage from "../pages/LobbyPage.jsx";
import LibraryPage from "../pages/LibraryPage.jsx";
import ConstructorPage from "../pages/ConstructorPage.jsx";
import {MenuItems} from "./Layout/Navigation/NavigationItemsData.js";
import NotFoundPage from "../pages/NotFoundPage.jsx";
import RegisterPage from "../pages/Authorization/RegisterPage.jsx";
import LoginPage from "../pages/Authorization/LoginPage.jsx";

const routeComponents = {
    '/': <HomePage/>,
    '/lobby': <LobbyPage/>,
    '/library': <LibraryPage/>,
    '/constructor': <ConstructorPage/>,
};

function App() {
    return (
        <>
            <Menu/>
            <Navigation/>
            <Container>
                <Routes>
                    {MenuItems.map(({href}) => (
                        <Route key={href} path={href} element={routeComponents[href]}/>
                    ))}
                    <Route path='/login' element={<LoginPage/>}></Route>
                    <Route path='/register' element={<RegisterPage/>}></Route>
                    <Route path="*" element={<NotFoundPage/>}></Route>
                </Routes>
            </Container>
        </>
    );
}

export default App
