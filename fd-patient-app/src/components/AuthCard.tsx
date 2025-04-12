import { useEffect, useState } from "react"
import { login } from '../service'
import {tokenExpiry} from '../utils'

interface AuthCardProps {
    onSuccesfulLogin: () => void
    onSuccesfulLogout: () => void
}

const AuthCard: React.FC<AuthCardProps> = ({ onSuccesfulLogin, onSuccesfulLogout }) => {
    const [username, setUserName] = useState<string | null>(null);
    const [roles, setRoles] = useState<string[]>([]);


    const handleLogout = () => {
        localStorage.clear();
        onSuccesfulLogout();
        setUserName(null);
    }

    const handleLogin = async (loginType: string) => {
        try {
            const response = await login(loginType);

            localStorage.setItem('token', response.token);
            localStorage.setItem('username', response.username);
            localStorage.setItem('roles', JSON.stringify(response.roles));

            setUserName(response.username);
            setRoles(response.roles || []);

            onSuccesfulLogin();
        } catch (err) {
            console.error('Login error:', err);
        }
    };

    useEffect(() => {
        const storedUsername = localStorage.getItem('username');
        const storedRoles = localStorage.getItem('roles');

        if (storedUsername) {
            setUserName(storedUsername);
            setRoles(storedRoles ? JSON.parse(storedRoles) : []);
        }
    }, []);

    const [timeLeft, setTimeLeft] = useState(tokenExpiry());

    useEffect(() => {
        const timer = setTimeout(() => {
            setTimeLeft(tokenExpiry(localStorage.getItem('token')));
        }, 1000);
    });


    return <>
        <div className="card text-center">
            <h2>Authentication</h2>
            {username &&
                <div>
                    <div>Logged in as {username}</div>
                    <p className="text-secondary">
                        <span className="fw-medium">Roles:</span> {roles.length > 0 ? roles.join(', ') : 'No roles'}
                        
                    </p>
                    <div>
                    <span className="fw-medium">Token Expiry time left in seconds:</span> 
                    <span className="fw-medium" style={{color : timeLeft < 0 ? 'green' : 'red'}}>{timeLeft}</span>
                    </div>

                    <div className="mb-4">
                        <button className="btn btn-danger" onClick={() => handleLogout()}>Logout</button>
                    </div>
                </div>
            }
            {!username &&
                <div className="mb-4">
                    <div>Login {username}</div>
                    <div>
                        <button
                            onClick={() => handleLogin('admin')}
                            className="btn btn-primary"
                        >
                            Login as Admin
                        </button>&nbsp;
                        <button
                            onClick={() => handleLogin('viewer')}
                            className="btn btn-secondary"
                        >
                            Login as Viewer
                        </button>&nbsp;
                        <button
                            onClick={() => handleLogin('unauthorized')}
                            className="btn btn-danger"
                        >
                            Login as Unauthorized
                        </button>&nbsp;

                    </div>
                </div>
            }
        </div>

    </>
}

export default AuthCard;