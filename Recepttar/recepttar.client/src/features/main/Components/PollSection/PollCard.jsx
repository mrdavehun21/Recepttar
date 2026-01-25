import { usePollVote } from '../../hooks/usePollVote';

function PollCard({ data }) {
    const { selectedOptionId, setSelectedOptionId, submitting, error, handleSubmit } =
        usePollVote();

    let AllowVote = true;

    return (
        <div
            className="card overflow-hidden shadow m-3"
            style={{ minWidth: '300px', width: '30%', maxWidth: '320px', height: '-webkit-fill-available' }}
        >
            <div className="card-body">
                <h4 className="card-title">{data.question}</h4>
                {error && <p className="text-danger">{error}</p>}
            </div>

            {data.options.map((option) => {
                const isSelected = selectedOptionId === option.id;
                if (option.votedOn != null) AllowVote = false;

                return (
                    <button
                        key={option.id}
                        onClick={() => setSelectedOptionId(option.id)}
                        className={`
              m-2 p-2 rounded-2 d-flex justify-content-between align-items-center
              ${isSelected ? 'border border-primary border-2 bg-info' : 'border-0'}
            `}
                    >
                        <span>{option.optionText}</span>
                        <span className="badge bg-secondary">{option.voteCount}</span>
                    </button>
                );
            })}

            <button
                className="w-100 text-light fw-bold border-0 bg-primary p-2"
                disabled={(submitting || !selectedOptionId) && AllowVote}
                onClick={() => handleSubmit(data.id)}
            >
                {submitting ? 'Submitting...' : 'Submit'}
            </button>
        </div>
    );
}

export default PollCard;
