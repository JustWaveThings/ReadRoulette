import React from 'react'
import ReactDOM from 'react-dom/client'
import App from './App.jsx'
import NotFound from './NotFound.jsx'
import {
    createBrowserRouter,
    RouterProvider,
} from "react-router-dom";
import './index.css'


const router = createBrowserRouter([
    {
        path: "/",
        element: <App/>
    },
    {
        path: "*",
        element: <NotFound/>
    }

])

ReactDOM.createRoot(document.getElementById('root')).render(
  <React.StrictMode>
    <RouterProvider router={router}/>
  </React.StrictMode>,
)
