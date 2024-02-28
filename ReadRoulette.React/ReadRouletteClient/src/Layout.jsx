import {Outlet, NavLink, ScrollRestoration} from "react-router-dom";
import Header from "./Header.jsx";
import Footer from "./Footer.jsx";

function Layout() {
    return (
        <>
            <Header/>
            <nav>
                <NavLink to="home"  end >Home</NavLink>
                <NavLink to="login"  end >Login</NavLink>
                <NavLink to="signup"  end >Signup</NavLink>
                <NavLink to="dashboard" end >Dashboard</NavLink>
            </nav>
            <Outlet/>
            <Footer/>
            <ScrollRestoration/>
        </>
    )
}

export default Layout