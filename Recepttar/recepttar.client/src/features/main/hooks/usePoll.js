import { useState, useEffect, useCallback } from 'react';
import { fetchActivePolls, getAuthorProfile, deletePoll } from '../api/main.api';

export function usePolls() {
    const [polls, setPolls] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    const fetchPollData = useCallback(async () => {
        setLoading(true);
        setError(null);

        try {
            let data = await fetchActivePolls();

            if (data[0].id == 0) {
                setPolls(data);
                return;
            }

            setPolls(data);
        } catch (err) {
            setError(err.message);
        } finally {
            setLoading(false);
        }
    }, []);

    const handleDeletePoll = async (pollId) => {
        try {
            await deletePoll(pollId)

            setPolls(prev => prev.filter(poll => poll.id !== pollId));
        } catch (error) { }
    }

    useEffect(() => {
        fetchPollData();
    }, [fetchPollData]);

    return {
        polls,
        loading,
        error,
        refetch: fetchPollData,
        deletePoll: handleDeletePoll
    };
}