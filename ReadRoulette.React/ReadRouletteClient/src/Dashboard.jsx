import { Outlet, Link} from "react-router-dom";
export default function Dashboard(){
    return (
        <div className="main dashboard">
            <h1>Dashboard</h1>
            <nav className="dash-nav">
                <Link to="/dashboard">Profile</Link>
                <Link to="booklist">Book List</Link>
            </nav>
            <Outlet/>
        </div>


    )
}