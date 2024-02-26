import {Outlet, Link} from "react-router-dom";

export default function BookList(){
    return (
        <>
            <h1>Book List</h1>
            <nav className="booklist-nav">
                <Link to="/dashboard/booklist">All Books</Link>
                <Link to="managebooklist">Manage Book List</Link>
            </nav>
            <Outlet/>
        </>
        )


}