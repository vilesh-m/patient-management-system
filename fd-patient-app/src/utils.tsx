import { jwtDecode } from 'jwt-decode';

export const tokenExpiry = (token?: string | null): number => {
    if (!token) {
        return 0
    }
    try {
        let decoded = jwtDecode(token);
        if (decoded.exp)
            return Math.floor(new Date().getTime()/1000 - decoded.exp);
        else
            return 0;
    } catch {
        return 0;
    }
};