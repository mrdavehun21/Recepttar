import { useState } from 'react';
import { usePollCard } from '../../hooks/usePollVote';

function PollCard({ data }) {
    const {
        question,
        options,
        hasVoted,
        selectedId,
        selectedOptionId,
        submitting,
        error,
        selectOption,
        submitVote
    } = usePollCard(data);

    return (
        <div
            className="card overflow-hidden shadow m-3"
            style={{ minWidth: '300px', width: '30%', maxWidth: '320px', height: '-webkit-fill-available' }}
        >
            <div className="card-body">
                <h4 className="card-title">{question}</h4>
                {error && <p className="text-danger">{error}</p>}
            </div>

            {options.map(option => {
                const isSelected = selectedId === option.id;

                return (
                    <button
                        key={option.id}
                        disabled={hasVoted}
                        onClick={() => selectOption(option.id)}
                        className={`
              m-2 p-2 rounded-2 d-flex justify-content-between align-items-center
              ${isSelected ? 'border border-primary border-2 bg-info' : 'border-0'}
            `}
                    >
                        <span>{option.optionText}</span>
                        <span className="badge bg-secondary">
                            {option.voteCount}
                        </span>
                    </button>
                );
            })}

            <button
                className="w-100 text-light fw-bold border-0 bg-primary p-2"
                disabled={hasVoted || submitting || !selectedOptionId}
                onClick={submitVote}
            >
                {hasVoted ? 'Already voted' : submitting ? 'Submitting...' : 'Submit'}
            </button>
        </div>
    );
}

export default PollCard;