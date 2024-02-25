import React from 'react'
import ReactDOM from 'react-dom/client'
import App from './App.jsx'
import Layout from './Layout.jsx'
import NotFound from './NotFound.jsx'
import Signup from "./Signup.jsx";
import Login from "./Login.jsx";
import {
    createBrowserRouter, createRoutesFromElements, Route,
    RouterProvider,
} from "react-router-dom";
import './index.css'
import requireAuth from "./requireAuth.js";
import Dashboard from "./Dashboard.jsx";


const router = createBrowserRouter(
    createRoutesFromElements(
        <Route
            path="/"
            element={<Layout />}
            // errorElement={<h1>Error</h1>}
            >
            <Route
                index
                element={<App/>}

            />
            <Route
                path="home"
                element={<App/>}

            />
            <Route
                path="login"
                element={<Login />}

            />
            <Route
                path="signup"
                element={<Signup/>}
            />
            <Route
                path="dashboard"
                element={<Dashboard/>}
                loader={async ({request}) => await requireAuth(request)}

            />
            <Route
                path="*"
                element={<NotFound />}

            />


        </Route>

))

ReactDOM.createRoot(document.getElementById('root')).render(
  <React.StrictMode>
    <RouterProvider router={router}/>
  </React.StrictMode>,
)
