import { useState } from 'react';
import { submitVote } from '../api/recipe.api';

export function usePollVote(initialSelected = null) {
    const [selectedOptionId, setSelectedOptionId] = useState(initialSelected);
    const [submitting, setSubmitting] = useState(false);
    const [error, setError] = useState(null);

    const handleSubmit = async (pollId) => {
        if (!selectedOptionId) {
            setError('Please select an option');
            return;
        }

        setSubmitting(true);
        setError(null);

        try {
            await submitVote(pollId, selectedOptionId);
        } catch (err) {
            setError(err.message);
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
