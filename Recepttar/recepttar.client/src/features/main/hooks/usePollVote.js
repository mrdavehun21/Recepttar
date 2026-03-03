import { useState } from 'react';
import { submitVote } from '../api/main.api';

export function usePollVote(initialSelected = null) {
    const [selectedOptionId, setSelectedOptionId] = useState(initialSelected);
    const [submitting, setSubmitting] = useState(false);
    const [error, setError] = useState({ errorCode: null, errorMessage: null });

    const handleSubmit = async (pollId) => {
        if (!selectedOptionId) {
            setError({
                errorCode: 402,
                errorMessage: 'Please select an option'
            });
            return false;
        }

        setSubmitting(true);
        setError({
            errorCode: null,
            errorMessage: null
        });

        try {
            await submitVote(pollId, selectedOptionId);
            return true;
        } catch (err) {
            switch (err.status) {
                case 401:
                    setError({
                        errorCode: err.status,
                        errorMessage: 'Sign in to vote!'
                    });
                    break;
                default:
                    setError({
                        errorCode: err.status,
                        errorMessage: 'Something went wrong!'
                    });
                    break;
            }
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
