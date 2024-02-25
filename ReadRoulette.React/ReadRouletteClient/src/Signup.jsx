import {Form } from 'react-router-dom'

export default function Signup(){
    return (
        <div>
            <h1>Signup</h1>
            <Form>
                <input type="text" placeholder="Username"/>
                <input type="password" placeholder="Password"/>
                <input type="submit" value="Signup"/>
            </Form>
        </div>
    )
}