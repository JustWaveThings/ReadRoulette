import { Form } from 'react-router-dom'

export default function Login(){
    return (
        <div className="main">
            <h1>Login</h1>
            <Form>
                <input type="text" placeholder="Username"/>
                <input type="password" placeholder="Password"/>
                <input type="submit" value="Login"/>
            </Form>
        </div>
    )
}