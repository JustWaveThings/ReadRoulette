import { Outlet, NavLink } from 'react-router-dom';
export default function BookClubs() {
    return(
        <>
            <h1>Book Clubs</h1>
            <nav className="bookclub-nav">
                <NavLink to="/dashboard/bookclubs" end >All Book Clubs</NavLink>
                <NavLink to="managebookclub" end >Manage Book Club</NavLink>
            </nav>
            <Outlet/>
        </>
    )

}