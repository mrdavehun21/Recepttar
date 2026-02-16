import { usePollCard } from '../../hooks/usePollVote';
import './PollCard.css';

function PollCard({ data, loginStatus }) {
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
        <div className="card overflow-hidden shadow m-3 rounded-3 vote-card">
            <div className="card-body mb-0 p-2 fw-bold">
                <p className="card-title fs-5 text-center mb-0 border-0 border-bottom border-black w-100 p-2">{question}</p>
                {
                    //(loginStatus) ? <></> : <p className="text-danger">{error.errorMessage == null ? "Sign in to vote!" : error.errorMessage }</p>
                }
            </div>

            {options.map(option => {
                const isSelected = selectedId === option.id;

                return (
                    <button
                        key={option.id}
                        disabled={hasVoted}
                        onClick={() => selectOption(option.id)}
                        className={"vote-btn polls-bg-hover-additional-5 polls-bg-additional-4 d-flex align-items-center p-0 m-2 border border-dark " + ((isSelected == true) ? ("polls-bg-additional-6") : (""))}
                    >
                        <span className="vote-text flex-grow-1 px-4 fs-6">
                            {option.optionText}
                        </span>

                        <div className="vote-divider" />

                        <span className="vote-count d-flex align-items-center justify-content-center">
                            {option.voteCount}
                        </span>
                    </button>
                );
            })}

            <button
                className={"w-100 text-light fw-bold border-0 polls-bg-additional-7 p-2 " + ((hasVoted) ? "polls-bg-additional-8" : "")}
                disabled={hasVoted || submitting || !selectedOptionId || !loginStatus}
                onClick={submitVote}
            >
                {hasVoted ? 'Already voted' : submitting ? 'Submitting...' : 'Submit'}
            </button>
        </div>
    );
}

export default PollCard;