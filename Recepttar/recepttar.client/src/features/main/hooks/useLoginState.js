import { useState, useEffect, useCallback } from 'react';
import { getUserProfile } from '../api/recipe.api';

export function useLoginStatus() {
    const [user, setUser] = useState([]);

    const fetchUserData = useCallback(async () => {
        try {
            const data = await getUserProfile();
            setUser(data);
        } catch (err) {
        } finally {
        }
    }, []);

    useEffect(() => {
        fetchUserData();
    }, [fetchUserData]);

    return {
        user
    };
}
