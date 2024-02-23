import {redirect} from "react-router-dom";

export default async function requireAuth(request){
    const pathName = new URL(request.url);
    const isLoggedIn = localStorage.getItem('isLoggedInRoulette');
    if(!isLoggedIn){
        throw redirect(`/login?redirectTo=${pathName.pathname}`);
    }
    return null;
}