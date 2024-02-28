import {Outlet, NavLink} from "react-router-dom";

export default function BookList(){
    return (
        <>
            <h1>Book List</h1>
            <nav className="booklist-nav">
                <NavLink to="/dashboard/booklist" end >All Books</NavLink>
                <NavLink to="managebooklist"  end >Manage Book List</NavLink>
            </nav>
            <Outlet/>
        </>
        )


}