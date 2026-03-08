import { useEffect, useState } from "react";
import { usePollCard } from '../../hooks/usePollVote';
import { ImageAvailable } from '../../../../GlobalHooks/usePictureChecker';
import './PollCard.css';

function PollCard({ data, loginStatus, profileID = null, deletePollMethod = null, openFormTrigger, returnPollValues }) {
    const API_BASE = import.meta.env.VITE_API_URL;

    const {
        question,
        options,
        hasVoted,
        selectedId,
        selectedOptionId,
        submitting,
        selectOption,
        submitVote
    } = usePollCard(data);

    const [imageExists, setImageExists] = useState(false);

    useEffect(() => {
        async function checkImage() {
            const exists = await ImageAvailable(
                `${API_BASE}/${data?.profilePicture}`
            );
            setImageExists(exists);
        }
        if (data?.profilePicture != undefined) {
            checkImage();
        }
    }, [data?.profilePicture]);

    return (
        <div className="card overflow-hidden shadow m-3 rounded-3 vote-card d-flex felx-column justify-content-between">
            <div>
                <div className="card-body mb-0 p-2 fw-bold border-0 border-bottom border-black w-100">
                    <p className="card-title fs-5 text-center mb-0 w-100 p-2 pb-0">{question}</p>
                </div>

                <a href={`https://localhost:65534/profile/${data?.authorId}`} className="text-decoration-none text-black">
                    <div className="card-body mb-0 p-2 fw-bold border-0 border-bottom border-black">
                        <div className="d-flex justify-content-center align-items-center">
                            {imageExists ? (
                                <>
                                    <img
                                        src={`${API_BASE}/${data?.profilePicture}`}
                                        width={36}
                                        height={36}
                                        className="rounded-circle me-2"
                                    />
                                    <span className="fw-semibold d-sm-block">
                                        {data?.fullName}
                                    </span>
                                </>
                            ) : (
                                <>
                                    <i className="bi bi-person-circle fs-2"></i>
                                    <span className="d-block align-self-center ms-2">
                                        {(data?.fullName == undefined) ? "unknown user" : data?.fullName}
                                    </span>
                                </>
                            )}
                        </div>
                    </div>
                </a>
            </div>

            <div>
                {
                    options.map(option => {
                        const isSelected = selectedId === option.optionId;

                        return (
                            <button
                                disabled={hasVoted || profileID?.id == data.authorId}
                                onClick={() => selectOption(option.optionId)}
                                className={"vote-btn polls-bg-hover-additional-5 polls-bg-additional-4 d-flex align-items-center p-0 m-2 border border-dark " + ((isSelected == true) ? ("polls-bg-additional-6") : (""))}
                                style={{ width: "-webkit-fill-available" }}
                                key={option.optionId}
                            >
                                <span className="vote-text flex-grow-1 px-4 fs-6">
                                    {option.optionText}
                                </span>

                                <span className="border-start border-black border-2 vote-count d-flex align-items-center justify-content-center" style={{ height: "-webkit-fill-available", margin: "10px" }}>
                                    {option.voteCount}
                                </span>
                            </button>
                        );
                    })
                }

                {
                    (profileID?.id == data.authorId) ? (
                        <div className="d-flex gap-1">
                            <button
                                className={"w-50 text-light fw-bold border-0 polls-bg-additional-7 p-2 "}
                                onClick={() => {openFormTrigger(); returnPollValues(data)}}
                            >
                                Edit
                            </button>
                            <button
                                className={"w-50 text-light fw-bold border-0 polls-bg-additional-7 p-2 "}
                                onClick={() => deletePollMethod(data.id)}
                            >
                                Delete
                            </button>
                        </div>
                    ): (
                        <button
                            className={"w-100 text-light fw-bold border-0 polls-bg-additional-7 p-2 " + ((hasVoted) ? "polls-bg-additional-8" : "")}
                            disabled={hasVoted || submitting || !selectedOptionId || !loginStatus}
                            onClick={submitVote}
                        >
                            {hasVoted ? 'Already voted' : 'Submit'}
                        </button>
                    )
                }

                
            </div>
        </div>
    );
}

export default PollCard;