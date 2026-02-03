import { useState, useEffect, useCallback } from 'react';
import { getLoginStatus } from '../GlobalApi/recipe.api';

export function useIsLoggedIn() {
    const [isLoggedIn, setIsLoggedIn] = useState(false);
    const [profileData, setProfileData] = useState([]);

    const checkLoginStatus = useCallback(async () => {
        try {
            const res = await getLoginStatus();
            setProfileData(res.data);
            setIsLoggedIn(true);
        } catch (err) {
            if (err?.response?.status === 401) {
                setIsLoggedIn(false);
            } else {
                setIsLoggedIn(false);
            }
        } 
    }, []);

    useEffect(() => {
        checkLoginStatus();
    }, [checkLoginStatus]);

    return {
        isLoggedIn,
        refetch: checkLoginStatus,
        profileData,
    };
}