import React from 'react'
import ReactDOM from 'react-dom/client'
import {
    createBrowserRouter, createRoutesFromElements, Route,
    RouterProvider,
} from "react-router-dom";
import './index.css'

import App from './App.jsx'
import Layout from './Layout.jsx'
import NotFound from './NotFound.jsx'
import Signup from "./Signup.jsx";
import Login from "./Login.jsx";
import requireAuth from "./requireAuth.js";
import Dashboard from "./Dashboard.jsx";
import BookList from "./BookList.jsx";
import ManageBooklist from "./ManageBooklist.jsx";
import ShowBooklist from "./ShowBooklist.jsx";

const router = createBrowserRouter(
    createRoutesFromElements(
        <Route
            path="/"
            element={<Layout />}
            errorElement={<h1>Error</h1>}
            >
            <Route
                index
                element={<App/>}
                errorElement={<h1>Error</h1>}
            />
            <Route
                path="home"
                element={<App/>}
                errorElement={<h1>Error</h1>}
            />
            <Route
                path="login"
                element={<Login />}
                errorElement={<h1>Error</h1>}
            />
            <Route
                path="signup"
                element={<Signup/>}
                errorElement={<h1>Error</h1>}
            />
            <Route
                path="*"
                element={<NotFound />}
                errorElement={<h1>Error</h1>}

            />
            <Route
                path="dashboard"
                element={<Dashboard/>}
                loader={async ({request}) => await requireAuth(request)}
                errorElement={<h1>Error</h1>}
            >
                <Route
                    index
                    element={<h1>Profile</h1>}
                    errorElement={<h1>Error</h1>}
                />
                <Route
                    path="booklist"
                    element={<BookList/>}
                    errorElement={<h1>Error</h1>}
                >
                    <Route
                        index
                        element={<ShowBooklist/>}
                        errorElement={<h1>Error</h1>}
                    />
                    <Route
                        path=":id"
                        element={<h1>Book</h1>}
                        errorElement={<h1>Error</h1>}
                    />
                    <Route
                        path= "managebooklist"
                        element={<ManageBooklist/>}
                        errorElement={<h1>Error</h1>}
                    />
                    <Route
                        path= "*"
                        element={<NotFound/>}
                        errorElement={<h1>Error</h1>}
                    />
                </Route>

                <Route
                    path="bookclubs"
                    element={<h1>All BookClubs</h1>}
                    errorElement={<h1>Error</h1>}
                />
                <Route
                    path="bookclubs/:id"
                    element={<h1>BookClub</h1>}
                    errorElement={<h1>Error</h1>}
                />
                <Route
                    path="bookclubs/:id/members"
                    element={<h1>Members</h1>}
                    errorElement={<h1>Error</h1>}
                />
                <Route
                    path="bookclubs/:id/books"
                    element={<h1>Book Club Book List</h1>}
                    errorElement={<h1>Error</h1>}
                />
                <Route
                    path="bookclubs/:id/books/:id"
                    element={<h1>Book Club Book List Specific Book</h1>}
                    errorElement={<h1>Error</h1>}
                />
                <Route
                    path= "*"
                    element={<NotFound/>}
                    errorElement={<h1>Error</h1>}
                />
            </Route>
        </Route>

))

ReactDOM.createRoot(document.getElementById('root')).render(
  <React.StrictMode>
    <RouterProvider router={router}/>
  </React.StrictMode>,
)
