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

export function usePollCard(data) {
    const [options, setOptions] = useState(data.options);
    const [votedOn, setVotedOn] = useState(data.votedOn);

    const {
        selectedOptionId,
        setSelectedOptionId,
        submitting,
        error,
        handleSubmit
    } = usePollVote();

    const hasVoted = votedOn !== null;
    const selectedId = hasVoted ? votedOn : selectedOptionId;

    const submitVote = async () => {
        const success = await handleSubmit(data.id);
        if (!success) return;

        setOptions(prev =>
            prev.map(opt =>
                opt.id === selectedOptionId
                    ? { ...opt, voteCount: opt.voteCount + 1 }
                    : opt
            )
        );

        setVotedOn(selectedOptionId);
    };

    return {
        question: data.question,
        options,
        hasVoted,
        selectedId,
        selectedOptionId,
        submitting,
        error,
        selectOption: setSelectedOptionId,
        submitVote
    };
}
