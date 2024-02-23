import {Outlet, Link} from "react-router-dom";

function Layout() {
    return (
        <div>
            <h1>Layout</h1>
            <nav>
                <Link to="home">Home</Link>
                <Link to="login">Login</Link>
            </nav>
            <div>
                <Outlet />
            </div>
        </div>
    )
}

export default Layout