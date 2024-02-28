import { Outlet, NavLink} from "react-router-dom";
export default function Dashboard(){
    return (
        <div className="main dashboard">
            <h1>Dashboard</h1>
            <nav className="dash-nav">
                <NavLink to="/dashboard" end>Profile</NavLink>
                <NavLink to="booklist"  end >Your Book List</NavLink>
                <NavLink to="bookclubs"  end >Book Clubs</NavLink>
            </nav>
            <Outlet/>
        </div>
    )
}