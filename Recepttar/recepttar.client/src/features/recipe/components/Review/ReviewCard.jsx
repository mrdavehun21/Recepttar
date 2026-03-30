import { useIsLoggedIn } from '../../../../shared/hooks/useLoginState';
import defaultAvatar from "../../../../assets/default-avatar.jpg";

const API_BASE = import.meta.env.VITE_API_URL;

const renderStars = (rating) => {
    return [1, 2, 3, 4, 5].map(star => (
        <i
            key={star}
            className={star <= rating ? 'bi bi-star-fill star-additional-9 me-2' : 'bi bi-star-fill star-additional-10 me-2'}
        ></i>
    ));
};

const ReviewCard = ({ review, onEditClick, onDeleteClick }) => {
    const { isLoggedIn, profileData } = useIsLoggedIn();
    const isOwner = isLoggedIn && profileData?.id === review.userId;

    return (
        <div className="card mb-3" style={{ border: 'none', borderRadius: '12px' }}>
            <div className="card-body">
                <div className="d-flex align-items-center justify-content-between mb-2">
                    <div className="d-flex align-items-center">
                        {review.profilePicture ? (
                            <img
                                src={`${API_BASE}/${review.profilePicture}`}
                                alt={review.fullName}
                                className="rounded-circle me-3 profile-picture-border"
                                style={{ width: '40px', height: '40px', objectFit: 'cover' }}
                                onError={(e) => { e.target.onerror = null; e.target.src = defaultAvatar; }}
                            />
                        ) : (
                            <div
                                className="rounded-circle bg-secondary me-3"
                                style={{ width: '40px', height: '40px', flexShrink: 0 }}
                            />
                        )}
                        <div>
                            <div className="fw-bold">{review.fullName}</div>
                            <small className="text-muted">
                                {new Date(review.createdAt).toLocaleDateString('hu-HU')}
                                {review.updatedAt && <span> - Updated: {new Date(review.updatedAt).toLocaleDateString('hu-HU')}</span>}
                            </small>
                        </div>
                    </div>
                    {isOwner && (
                        <div className="d-flex gap-2">
                            <button className="btn btn-sm btn-link text-primary p-0" onClick={() => onEditClick(review)}>
                                <i className="bi bi-pencil-square" />
                            </button>
                            <button className="btn btn-sm btn-link text-danger p-0" onClick={() => onDeleteClick(review)}>
                                <i className="bi bi-x-lg" />
                            </button>
                        </div>
                    )}
                </div>
                <div className="d-flex justify-content-center">
                    <div className="mb-2">{renderStars(review.stars)}</div>
                </div>
                <textarea
                    className="form-control review-bg"
                    value={review.comment}
                    readOnly
                    style={{ resize: 'none' }}
                    rows={4}
                />
            </div>
        </div>
    );
};

export default ReviewCard;