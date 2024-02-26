import { Outlet} from "react-router-dom";
export default function Dashboard(){
    return (
        <div className="main">
            <h1>Dashboard</h1>
            <Outlet/>
        </div>


    )
}