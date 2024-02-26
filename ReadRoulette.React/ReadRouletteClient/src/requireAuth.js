import {redirect} from "react-router-dom";

export default async function requireAuth(request){
    const pathName = new URL(request.url);
    //const isLoggedIn = localStorage.getItem('isLoggedInRoulette');
    const isLoggedIn = true;
    if(!isLoggedIn){
        throw redirect(`/login?redirectTo=${pathName.pathname}`);
    }
    return null;
}