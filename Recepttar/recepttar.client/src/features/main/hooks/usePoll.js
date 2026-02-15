import { useState, useEffect, useCallback } from 'react';
import { fetchActivePolls } from '../api/main.api';

export function usePolls() {
    const [polls, setPolls] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    const fetchPollData = useCallback(async () => {
        setLoading(true);
        setError(null);

        try {
            const data = await fetchActivePolls();
            setPolls(data);
        } catch (err) {
            setError(err.message);
        } finally {
            setLoading(false);
        }
    }, []);

    useEffect(() => {
        fetchPollData();
    }, [fetchPollData]);

    return {
        polls,
        loading,
        error,
        refetch: fetchPollData
    };
}
