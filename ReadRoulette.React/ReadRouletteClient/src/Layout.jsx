import {Outlet, Link} from "react-router-dom";

function Layout() {
    return (
        <div>
            <nav>
                <Link to="home">Home</Link>
                <Link to="login">Login</Link>
                <Link to="signup">Signup</Link>
                <Link to="dashboard">Dashboard</Link>
            </nav>
            <div>
                <Outlet />
            </div>
        </div>
    )
}

export default Layout