import { createContext, useContext, useEffect, useState } from "react";
import { getLoginStatus } from "../GlobalApi/recipe.api";

const AuthContext = createContext();

export function AuthProvider({ children }) {
    const [isLoggedIn, setIsLoggedIn] = useState(null);
    const [profileData, setProfileData] = useState(null);

    const checkLoginStatus = async () => {
        try {
            const res = await getLoginStatus();
            setProfileData(res.data);
            setIsLoggedIn(true);
        } catch {
            setIsLoggedIn(false);
            setProfileData(null);
        }
    };

    useEffect(() => {
        checkLoginStatus();
    }, []);

    return (
        <AuthContext.Provider value={{ isLoggedIn, profileData, refetch: checkLoginStatus }}>
            {children}
        </AuthContext.Provider>
    );
}

export function useAuth() {
    return useContext(AuthContext);
}