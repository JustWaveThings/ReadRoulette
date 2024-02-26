import { Outlet, Link } from 'react-router-dom';
export default function BookClubs() {
    return(
        <>
            <h1>Book Clubs</h1>
            <nav className="bookclub-nav">
                <Link to="/dashboard/bookclubs">All Book Clubs</Link>
                <Link to="managebookclub">Manage Book Club</Link>
            </nav>
            <Outlet/>
        </>
    )

}