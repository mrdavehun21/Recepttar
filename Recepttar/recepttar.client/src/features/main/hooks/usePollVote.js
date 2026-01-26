import { useState } from 'react';
import { submitVote } from '../api/recipe.api';

export function usePollVote(initialSelected = null) {
    const [selectedOptionId, setSelectedOptionId] = useState(initialSelected);
    const [submitting, setSubmitting] = useState(false);
    const [error, setError] = useState(null);

    const handleSubmit = async (pollId) => {
        if (!selectedOptionId) {
            setError('Please select an option');
            return false;
        }

        setSubmitting(true);
        setError(null);

        try {
            await submitVote(pollId, selectedOptionId);
            return true;
        } catch (err) {
            setError(err.message);
            return false;
        } finally {
            setSubmitting(false);
        }
    };


    return {
        selectedOptionId,
        setSelectedOptionId,
        submitting,
        error,
        handleSubmit
    };
}
