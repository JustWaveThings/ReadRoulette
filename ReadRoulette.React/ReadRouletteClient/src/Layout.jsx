import {Outlet, Link, ScrollRestoration} from "react-router-dom";
import Header from "./Header.jsx";
import Footer from "./Footer.jsx";

function Layout() {
    return (
        <>
            <Header/>
            <nav>
                <Link to="home">Home</Link>
                <Link to="login">Login</Link>
                <Link to="signup">Signup</Link>
                <Link to="dashboard">Dashboard</Link>
            </nav>
            <Outlet/>
            <Footer/>
            <ScrollRestoration/>
        </>
    )
}

export default Layout