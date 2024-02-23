import React from 'react'
import ReactDOM from 'react-dom/client'
import App from './App.jsx'
import Layout from './Layout.jsx'
import NotFound from './NotFound.jsx'
import Home from './Home.jsx'
import {
    createBrowserRouter, createRoutesFromElements, Route,
    RouterProvider,
} from "react-router-dom";
import './index.css'


const router = createBrowserRouter(
    createRoutesFromElements(
        <Route
            path="/"
            element={<Layout />}
            errorElement={<h1>Error</h1>}
            >
            <Route
                path="home"
                element={<App/>}
            />
            <Route
                path="login"
                element={<h1>Login</h1>}
            />
            <Route
                path="dashboard"
                element={<h1>Dashboard</h1>}
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
