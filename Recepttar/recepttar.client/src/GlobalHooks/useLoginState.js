import { useState, useEffect, useCallback } from 'react';
import { getLoginStatus } from '../GlobalApi/recipe.api';

export function useIsLoggedIn() {
    const [isLoggedIn, setIsLoggedIn] = useState(null);
    const [profileData, setProfileData] = useState(null);

    const checkLoginStatus = useCallback(async () => {
        try {
            const res = await getLoginStatus();
            setProfileData(res.data);
            setIsLoggedIn(true);
        } catch (err) {
            if (err?.response?.status === 401) {
                setIsLoggedIn(false);
                setProfileData(null);
            } else {
                setIsLoggedIn(false);
                setProfileData(null);
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